"use client";

import React, { useState, useEffect, useRef } from 'react';
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Label } from "@/components/ui/label";
import api from "@/services/axiosApi";
import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Camera, Mic, MicOff, Video, VideoOff, Monitor, Settings, Users, MessageSquare } from 'lucide-react';
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { ScrollArea } from "@/components/ui/scroll-area";
import Hls from 'hls.js';

export default function HLSBroadcast() {
  const [isVideoOn, setIsVideoOn] = useState(true);
  const [isMuted, setIsMuted] = useState(false);
  const [seconds, setSeconds] = useState(0);
  const [isActive, setIsActive] = useState(false);
  const [mediaStream, setMediaStream] = useState<MediaStream | null>(null);
  const [streamKey, setStreamKey] = useState<string | null>(null);
  const [streamUrl, setStreamUrl] = useState<string | null>(null);
  const [streamTitle, setStreamTitle] = useState("");
  const [streamDescription, setStreamDescription] = useState("");
  const [chatMessages, setChatMessages] = useState<string[]>([]);
  const [newChatMessage, setNewChatMessage] = useState("");
  const [mediaRecorder, setMediaRecorder] = useState<MediaRecorder | null>(null);
  const [isRecording, setIsRecording] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const videoRef = useRef<HTMLVideoElement | null>(null);
  const hlsRef = useRef<Hls | null>(null);

  useEffect(() => {
    if (isActive) {
      const interval = setInterval(() => setSeconds((prev) => prev + 1), 1000);
      return () => clearInterval(interval);
    }
  }, [isActive]);

  useEffect(() => {
    if (mediaStream && videoRef.current) {
      videoRef.current.srcObject = mediaStream;
    }
  }, [mediaStream]);

  const handleDataAvailable = async (event: BlobEvent) => {
    if (event.data && event.data.size > 0) {
      try {
        console.log(`Sending video chunk: ${event.data.size} bytes`);
        
        // Create a binary blob with WebM content
        const chunk = new Blob([event.data], { type: 'video/webm' });
        
        // Send chunk to server
        await api.post(`/HLS/stream/${streamKey}/data`, chunk, {
          headers: {
            'Content-Type': 'video/webm',
          },
          onUploadProgress: (progressEvent) => {
            // const percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total);
            // console.log(`Upload progress: ${percentCompleted}%`);
          },
        });

        console.log('Chunk sent successfully');
      } catch (err: unknown) {
        const error = err as Error;
        console.error('Error sending video chunk:', error);
      }
    }
  };

  const startRecording = async () => {
    try {
      // Get the current media stream from state
      const currentStream = mediaStream;
      console.log("Current media stream state:", {
        exists: !!currentStream,
        active: currentStream?.active,
        tracks: currentStream?.getTracks().map(t => ({
          kind: t.kind,
          enabled: t.enabled,
          muted: t.muted,
          readyState: t.readyState
        }))
      });

      if (!currentStream) {
        console.error('No media stream available');
        throw new Error('No media stream available. Please make sure screen sharing is active.');
      }

      if (!currentStream.active) {
        console.error('Media stream is not active');
        throw new Error('Media stream is not active. Please try sharing your screen again.');
      }

      // Check if we have video track
      const videoTrack = currentStream.getVideoTracks()[0];
      if (!videoTrack || videoTrack.readyState !== 'live') {
        console.error('No valid video track available', {
          exists: !!videoTrack,
          readyState: videoTrack?.readyState
        });
        throw new Error('No valid video track available. Please check your screen sharing settings.');
      }

      console.log('Starting recording with tracks:', {
        video: currentStream.getVideoTracks().length > 0,
        audio: currentStream.getAudioTracks().length > 0,
        videoSettings: videoTrack.getSettings()
      });

      const options: MediaRecorderOptions = {
        mimeType: 'video/webm;codecs=vp8,opus',
        videoBitsPerSecond: 1000000, // 1 Mbps
      };

      // Create new MediaRecorder
      const recorder = new MediaRecorder(currentStream, options);
      console.log('MediaRecorder created with options:', {
        mimeType: recorder.mimeType,
        state: recorder.state
      });

      // Set up data handler
      recorder.ondataavailable = handleDataAvailable;
      
      // Start recording with 1 second chunks
      recorder.start(1000);
      console.log('MediaRecorder started');
      
      setIsRecording(true);
      setMediaRecorder(recorder);

      // Set up error handler
      recorder.onerror = (event) => {
        console.error('MediaRecorder error:', event);
        setError('Recording error occurred. Please try again.');
      };
    } catch (err: unknown) {
      const error = err as Error;
      console.error('Error starting recording:', error);
      throw error;
    }
  };

  const startStream = async () => {
    let localStream: MediaStream | undefined = undefined;
    try {
      console.log("Starting stream...");
      
      // Get display media stream
      try {
        const stream = await navigator.mediaDevices.getDisplayMedia({ 
          video: { 
            width: { ideal: 1920 },
            height: { ideal: 1080 },
            frameRate: { ideal: 30 }
          },
          audio: true
        } as DisplayMediaStreamOptions);
        
        localStream = stream;
        
        // Set media stream state immediately after getting it
        setMediaStream(localStream);
        
        console.log("Display media obtained:", {
          video: localStream.getVideoTracks().length > 0,
          audio: localStream.getAudioTracks().length > 0,
          active: localStream.active
        });

        // Connect stream to video preview immediately
        if (videoRef.current) {
          videoRef.current.srcObject = localStream;
          videoRef.current.muted = true;
          console.log("Connected stream to video preview");
        } else {
          console.warn("Video reference not available");
        }

        // Try to get microphone audio if no audio track is present
        if (localStream.getAudioTracks().length === 0) {
          try {
            console.log("No audio track, attempting to get microphone access...");
            const audioStream = await navigator.mediaDevices.getUserMedia({
              audio: {
                echoCancellation: true,
                noiseSuppression: true,
                sampleRate: 44100,
                autoGainControl: true
              }
            });
            
            // Add audio tracks to the stream
            audioStream.getAudioTracks().forEach(track => {
              console.log('Adding audio track:', {
                kind: track.kind,
                label: track.label,
                enabled: track.enabled
              });
              if (localStream) {
                localStream.addTrack(track);
              }
            });
          } catch (micErr) {
            const micError = micErr as Error;
            console.warn("Could not get microphone access:", micError.message);
          }
        }

        // Generate stream key and start server-side stream
        const key = Math.random().toString(36).substring(2, 15);
        setStreamKey(key);
        console.log("Generated stream key:", key);

        console.log("Sending start stream request to server...");
        const response = await api.post("/HLS/start", {
          streamKey: key
        });
        console.log("Server response:", response.data);
        setStreamUrl(response.data.streamUrl);

        // Wait for stream initialization
        console.log("Waiting for stream initialization");
        let status = null;
        let attempts = 0;
        while (attempts < 10) {
          const statusResponse = await api.get(`/HLS/stream/${key}/status`);
          status = statusResponse.data;
          console.log("Stream status:", status);
          
          if (status.status === "active") {
            break;
          }
          
          await new Promise(resolve => setTimeout(resolve, 500));
          attempts++;
        }

        if (!status || status.status !== "active") {
          throw new Error("Stream failed to initialize on server");
        }

        // Start recording
        console.log("Starting recording...");
        await startRecording();
        console.log("Recording started successfully");

        // Initialize HLS player
        if (Hls.isSupported()) {
          hlsRef.current = new Hls({
            debug: true,
            enableWorker: true,
            lowLatencyMode: true
          });

          if (videoRef.current) {
            hlsRef.current.attachMedia(videoRef.current);
            hlsRef.current.loadSource(response.data.streamUrl);
            console.log("HLS initialized with stream URL:", response.data.streamUrl);
          } else {
            console.warn("Video reference not available for HLS");
          }
        } else {
          console.warn("HLS.js is not supported in this browser");
        }

        setIsActive(true);
        console.log("Stream started successfully");
      } catch (err: unknown) {
        const error = err as Error;
        console.error("Error in stream setup:", error);
        throw error;
      }
    } catch (err: unknown) {
      const error = err as Error;
      console.error("Error starting stream:", error);
      
      // Cleanup
      if (localStream && localStream.getTracks) {
        console.log("Cleaning up media stream...");
        localStream.getTracks().forEach(track => {
          track.stop();
          console.log(`Stopped track: ${track.kind}`);
        });
      }
      setMediaStream(null);
      setError(error.message || "Failed to start stream");
    }
  };

  const stopStream = async () => {
    try {
      if (hlsRef.current) {
        hlsRef.current.destroy();
        hlsRef.current = null;
      }
      
      if (streamKey) {
        console.log("Stopping stream:", streamKey);
        await api.post(`/HLS/stop/${streamKey}`);
      }
      
      // Stop MediaRecorder
      if (mediaRecorder && mediaRecorder.state !== 'inactive') {
        mediaRecorder.stop();
        setMediaRecorder(null);
        console.log("MediaRecorder stopped");
      }

      // Stop all tracks
      if (mediaStream) {
        mediaStream.getTracks().forEach(track => track.stop());
        setMediaStream(null);
        console.log("Media tracks stopped");
      }
      
      setIsActive(false);
      setSeconds(0);
      setStreamKey(null);
      setStreamUrl(null);
      console.log("Stream state reset");
    } catch (err: unknown) {
      const error = err as Error;
      console.error("Error stopping stream:", error);
    }
  };

  const toggleMute = () => {
    setIsMuted(prev => !prev);
    if (mediaStream) {
      mediaStream.getAudioTracks().forEach(track => {
        track.enabled = isMuted;
      });
    }
  };

  const toggleCamera = async () => {
    setIsVideoOn(prev => !prev);
    if (isVideoOn) {
      if (mediaStream) {
        mediaStream.getVideoTracks().forEach(track => track.stop());
      }
    } else {
      try {
        const newStream = await navigator.mediaDevices.getUserMedia({ video: true });
        if (mediaStream) {
          const audioTracks = mediaStream.getAudioTracks();
          audioTracks.forEach(track => newStream.addTrack(track));
        }
        setMediaStream(newStream);
      } catch (err: unknown) {
        const error = err as Error;
        console.error("Error accessing camera:", error);
      }
    }
  };

  const toggleScreenShare = async () => {
    try {
      const screenStream = await navigator.mediaDevices.getDisplayMedia({
        video: {
          width: { ideal: 1920 },
          height: { ideal: 1080 },
          frameRate: { ideal: 30 }
        }
      });
      
      if (mediaStream) {
        const audioTracks = mediaStream.getAudioTracks();
        audioTracks.forEach(track => screenStream.addTrack(track));
      }
      
      setMediaStream(screenStream);

      // Update MediaRecorder with new stream
      if (mediaRecorder) {
        mediaRecorder.stop();
        const newRecorder = new MediaRecorder(screenStream, {
          mimeType: 'video/webm;codecs=vp8,opus',
          videoBitsPerSecond: 1000000,
          audioBitsPerSecond: 128000
        });
        
        newRecorder.ondataavailable = handleDataAvailable;
        
        newRecorder.start(1000);
        setMediaRecorder(newRecorder);
      }
    } catch (err: unknown) {
      const error = err as Error;
      console.error("Error accessing screen share:", error);
    }
  };

  const sendChatMessage = () => {
    if (newChatMessage.trim()) {
      setChatMessages(prev => [...prev, newChatMessage]);
      setNewChatMessage("");
    }
  };
  return (
    <div className="container mx-auto p-4">
      <Card className="w-full max-w-4xl mx-auto">
        <CardHeader>
          <CardTitle className="text-2xl font-bold">Start a New Stream</CardTitle>
        </CardHeader>
        <CardContent>
          <Tabs defaultValue="setup">
            <TabsList className="grid w-full grid-cols-3">
              <TabsTrigger value="setup">Stream Setup</TabsTrigger>
              <TabsTrigger value="preview">Preview</TabsTrigger>
              <TabsTrigger value="chat">Live Chat</TabsTrigger>
            </TabsList>
            <TabsContent value="setup" className="space-y-4">
              <Label htmlFor="title">Stream Title</Label>
              <Input 
                id="title" 
                value={streamTitle} 
                onChange={(e) => setStreamTitle(e.target.value)} 
                placeholder="Enter your stream title" 
              />
              <Label htmlFor="description">Stream Description</Label>
              <Textarea 
                id="description" 
                value={streamDescription} 
                onChange={(e) => setStreamDescription(e.target.value)} 
                placeholder="Describe your stream" 
                rows={4} 
              />
              {streamUrl && (
                <div className="mt-4">
                  <Label>Stream URL</Label>
                  <Input value={streamUrl} readOnly />
                </div>
              )}
            </TabsContent>
            <TabsContent value="preview">
              <div className="aspect-video bg-muted">
                <video 
                  ref={videoRef} 
                  autoPlay 
                  playsInline 
                  muted 
                  className="w-full h-full object-cover" 
                />
              </div>
              <div className="flex justify-center space-x-2 mt-4">
                <Button onClick={toggleCamera} variant="outline" size="icon">
                  {isVideoOn ? <Video size="24" /> : <VideoOff size="24" />}
                </Button>
                <Button onClick={toggleMute} variant="outline" size="icon">
                  {isMuted ? <MicOff size="24" /> : <Mic size="24" />}
                </Button>
                <Button onClick={toggleScreenShare} variant="outline" size="icon">
                  <Monitor size="24" />
                </Button>
              </div>
              {isActive && (
                <div className="mt-4 text-center">
                  <p>Stream Duration: {Math.floor(seconds / 3600)}:{Math.floor((seconds % 3600) / 60)}:{seconds % 60}</p>
                </div>
              )}
            </TabsContent>
            <TabsContent value="chat">
              <ScrollArea className="h-[300px] w-full rounded-md border p-4">
                {chatMessages.map((message, index) => (
                  <div key={index} className="mb-2 p-2 bg-gray-100 rounded-md">
                    {message}
                  </div>
                ))}
              </ScrollArea>
              <div className="mt-4 flex space-x-2">
                <Input 
                  value={newChatMessage} 
                  onChange={(e) => setNewChatMessage(e.target.value)} 
                  placeholder="Type your message..." 
                  onKeyPress={(e) => e.key === "Enter" && sendChatMessage()} 
                />
                <Button onClick={sendChatMessage}>
                  <MessageSquare size="16" className="mr-2" />
                  Send
                </Button>
              </div>
            </TabsContent>
          </Tabs>
        </CardContent>
        <CardFooter className="flex justify-between items-center">
          <Button 
            onClick={startStream} 
            disabled={isActive || !streamTitle.trim()}
          >
            Start Stream
          </Button>
          <Button 
            onClick={stopStream} 
            disabled={!isActive}
          >
            Stop Stream
          </Button>
        </CardFooter>
      </Card>
      {error && (
        <div className="mt-4 text-red-500">{error}</div>
      )}
    </div>
  );
}
