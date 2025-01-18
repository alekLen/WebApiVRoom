'use client'

import React, { useState, useEffect } from 'react'
import Image from "next/image"
import { useUser } from '@clerk/nextjs'
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Label } from "@/components/ui/label"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"
import { Checkbox } from "@/components/ui/checkbox"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { IVideo } from "@/types/videoinfo.interface"
import { IChannel } from "@/types/channelinfo.interface"
import api from '@/services/axiosApi'
import { Upload, X, Plus } from 'lucide-react'
import { ScrollArea } from "@/components/ui/scroll-area"
import imageCompression from 'browser-image-compression';

interface IChannelEditProps {
    params: { locale: string }
}

interface Category {
    id: number
    name: string
    videosId: number[]
}

interface Tag {
    id: number
    name: string
    videosId: number[]
}

const VideoUpdateInterface: React.FC = () => {
    const [video, setVideo] = useState<IVideo | null>(null)
    const [availableVideos, setAvailableVideos] = useState<IVideo[]>([])
    const [isCategoryOpen, setIsCategoryOpen] = useState(false)
    const [visibility, setVisibility] = useState('private')
    const [isAgeRestricted, setAgeRestricted] = useState(false)
    const [videoName, setTitle] = useState('')
    const [description, setDescription] = useState('')
    const [category, setCategory] = useState('')
    const [categories, setCategories] = useState<Category[]>([])
    const [thumbnail, setThumbnail] = useState<File | null>(null)
    const [thumbnailPreview, setThumbnailPreview] = useState<string>("")
    const [thumbnailBase64, setThumbnailBase64] = useState<string>("")
    const [newCategory, setNewCategory] = useState('')
    const [currentTag, setCurrentTag] = useState('')
    const [isCopyright, setIsCopyright] = useState<boolean>(false)
    const [audience, setAudience] = useState<string>('all')
    const [userChannel, setUserChannel] = useState<IChannel | null>(null)
    const [selectedCategoryId, setSelectedCategoryId] = useState<number | null>(null)
    const [tagIds, setTagIds] = useState<number[]>([])
    const [tags, setTags] = useState<Tag[]>([])
    const { user } = useUser()
    const [isUpdating, setIsUpdating] = useState(false);
  const [updateProgress, setUpdateProgress] = useState(0);
  const [updateComplete, setUpdateComplete] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [deleteError, setDeleteError] = useState<string | null>(null);

  useEffect(() => {
    if (user) {
      fetchAvailableVideos();
      fetchCategories();
      getUserChannel();
    }
  }, [user]);

  useEffect(() => {
    if (userChannel) {
      fetchAvailableVideos();
    }
  }, [userChannel]);

  const fetchAvailableVideos = async () => {
    if (!userChannel?.id) {
      console.error('User channel ID is missing');
      return;
    }
    try {
      const response = await api.get(`/Video/getchannelvideos/${userChannel.id}`);
      setAvailableVideos(response.data);
    } catch (error) {
      console.error('Error fetching available videos:', error);
    }
  };

  const fetchCategories = async () => {
    try {
      const response = await api.get('/Category');
      setCategories(response.data);
    } catch (error) {
      console.error('Error fetching categories:', error);
    }
  };

  const getUserChannel = async () => {
    if (user) {
      try {
        const response = await api.get(`/ChannelSettings/getbyownerid/${user.id}`);
        if (response.status === 200) {
          setUserChannel(response.data);
        }
      } catch (error) {
        console.error('Error fetching user channel:', error);
      }
    }
  };

  const addCategory = () => {
    if (newCategory && !categories.some((category) => category.name === newCategory)) {
      const newCategoryObj: Category = {
        id: 0,
        name: newCategory,
        videosId: [],
      };
      setCategories([...categories, newCategoryObj]);
      DownloadCategory(newCategoryObj);
      setNewCategory('');
    }
  };

  const DownloadCategory = async (category: Category) => {
    try {
      const response = await api.post('/Category/add', category, {
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (response.status !== 200) {
        throw new Error('Failed to add category');
      }
      console.log('Category added successfully');
      await fetchCategories();
      return response.data.id;
    } catch (error) {
      console.error('Error adding category:', error);
    }
  };

  const handleVideoSelect = (video: IVideo) => {
    try {
      setVideo(video); // Update video
      setTitle(video.tittle);
      setDescription(video.description);
      setVisibility(video.visibility ? 'public' : 'private');
      setAgeRestricted(video.isAgeRestriction);
      setIsCopyright(video.isCopyright);
      setAudience(video.audience);
      setThumbnailPreview(`data:image/jpeg;base64,${video.cover}`);
      console.log('Video data:', video); // Check video data
    } catch (error) {
      console.error('Error selecting video:', error);
    }
  };

  const handleTagInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setCurrentTag(e.target.value);
  };

  const addTag = async (tagName: string) => {
    if (tagName && !tags.some(tag => tag.name === tagName)) {
      const newTagObj: Tag = {
        id: 0,
        name: tagName,
        videosId: [],
      };

      setTags(prevTags => [...prevTags, newTagObj]);

      try {
        await api.post('/Tag/add', newTagObj);
        await fetchTagIdByName(tagName);
      } catch (error) {
        console.error('Error adding tag:', error);
      } finally {
        setCurrentTag('');
      }
    }
  };

  const fetchTagIdByName = async (tagName: string): Promise<void> => {
    try {
      const response = await api.get(`/Tag/getbytagname/${tagName}`);
      if (response.status !== 200 || !response.data.id) {
        throw new Error('Failed to fetch tag ID');
      }

      const tagId = response.data.id;

      setTagIds(prevIds => {
        if (!prevIds.includes(tagId)) {
          return [...prevIds, tagId];
        }
        return prevIds;
      });

      setTags(prevTags =>
        prevTags.map(tag =>
          tag.name === tagName ? { ...tag, id: tagId } : tag
        )
      );
    } catch (error) {
      console.error('Error fetching tag ID:', error);
      throw error;
    }
  };

  const removeTag = (id: number) => {
    setTags((prevTags) => prevTags.filter((tag) => tag.id !== id));
    setTagIds((prevIds) => prevIds.filter((tagId) => tagId !== id));
  };

  const handleThumbnailChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];

    if (file && (file.type === "image/png" || file.type === "image/jpeg" || file.type === "image/jpg")) {
      setThumbnail(file);

      const reader = new FileReader();
      reader.onload = () => {
        const base64String = reader.result?.toString() || "";
        console.log("Generated Base64:", base64String);
        setThumbnailBase64(base64String);
        setThumbnailPreview(base64String);
      };
      reader.readAsDataURL(file);
    } else {
      console.error("Unsupported file format. Please select a PNG or JPEG image.");
    }
  };

  const handleVideoDelete = async (id: number): Promise<void> => {
    if (!id) {
      console.error('Invalid video ID');
      return;
    }

    if (!window.confirm('Are you sure you want to delete this video? This action cannot be undone.')) {
      return;
    }

    setIsDeleting(true);
    setDeleteError(null);

    try {
      console.log('Attempting to delete video:', id);
      const response = await api.delete(`/Video/${id}`);
      console.log('Delete response:', response);
      
      if (response.status === 204) {
        // Update the UI only after successful deletion
        setAvailableVideos(prevVideos => prevVideos.filter(v => v.id !== id));
        
        // Reset selected video if it was deleted
        if (video?.id === id) {
          setVideo(null);
          setTitle('');
          setDescription('');
          setThumbnailPreview('');
          setThumbnailBase64('');
        }
        
        alert('Video successfully deleted');
      }
    } catch (error: any) {
      console.error('Error deleting video:', error);
      let errorMessage = 'Failed to delete video';
      
      if (error.response) {
        // The request was made and the server responded with a status code
        // that falls out of the range of 2xx
        if (error.response.status === 404) {
          errorMessage = 'Video not found';
        } else if (error.response.data?.message) {
          errorMessage = error.response.data.message;
        } else if (error.response.status === 500) {
          errorMessage = 'Server error occurred while deleting video';
        }
      } else if (error.request) {
        // The request was made but no response was received
        errorMessage = 'No response received from server';
      } else {
        // Something happened in setting up the request that triggered an Error
        errorMessage = error.message || 'An unexpected error occurred';
      }
      
      setDeleteError(errorMessage);
      alert(`Error: ${errorMessage}`);
    } finally {
      setIsDeleting(false);
    }
  };

  const handleSubmit = async () => {
    if (!video) {
      console.error('No video data available');
      return;
    }

    setIsUpdating(true);
    setUpdateProgress(0);
    setUpdateComplete(false);

    try {
      // Перевіряємо наявність обов'язкових полів
      if (!videoName && !video.tittle) {
        throw new Error('Video title is required');
      }

      const updatedVideoData = {
        id: video.id,
        objectID: video.objectID,
        channelSettingsId: video.channelSettingsId,
        tittle: videoName || video.tittle,
        description: description || video.description,
        uploadDate: video.uploadDate,
        duration: video.duration,
        videoUrl: video.videoUrl,
        vRoomVideoUrl: video.vRoomVideoUrl,
        viewCount: video.viewCount,
        likeCount: video.likeCount,
        dislikeCount: video.dislikeCount,
        isShort: video.isShort,
        cover: thumbnailBase64 
          ? thumbnailBase64.replace(/^data:image\/[a-z]+;base64,/, '')
          : video.cover,
        visibility: visibility === 'public',
        isAgeRestriction: isAgeRestricted,
        isCopyright: isCopyright,
        audience: audience || 'all',
        categoryIds: selectedCategoryId ? [selectedCategoryId] : [],
        tagIds: tagIds || [],
        historyOfBrowsingIds: video.historyOfBrowsingIds || [],
        commentVideoIds: video.commentVideoIds || [],
        playLists: video.playLists || [],
        lastViewedPosition: video.lastViewedPosition || '00:00:00'
      };

      // Логуємо дані перед відправкою
      console.log('Sending update request with data:', {
        ...updatedVideoData,
        cover: updatedVideoData.cover ? 'base64_data' : 'no_cover' // Не логуємо повний base64
      });

      const response = await api.put(`/Video/update`, updatedVideoData, {
        onUploadProgress: (progressEvent) => {
          const percentCompleted = Math.round(
            (progressEvent.loaded * 100) / progressEvent.total!
          );
          setUpdateProgress(percentCompleted);
        },
      });

      if (response.status !== 200) {
        throw new Error(`Failed to update video: ${response.statusText}`);
      }

      console.log('Video updated successfully:', response.data);
      setVideo(response.data);
      setUpdateComplete(true);
    } catch (error: any) {
      console.error('Error updating video data:', {
        message: error.message,
        response: error.response?.data,
        status: error.response?.status
      });
      // Можна додати відображення помилки для користувача
    } finally {
      setIsUpdating(false);
    }
  };
    return (
        <div className="container p-4">
            <h1 className="text-2xl font-bold mb-6">Update Video</h1>
            <Dialog>
                <DialogTrigger asChild>
                    <Button className="mb-6">Select Video to Update</Button>
                </DialogTrigger>
                <DialogContent className="sm:max-w-[425px]">
                    <DialogHeader>
                        <DialogTitle>Select a Video</DialogTitle>
                    </DialogHeader>
                    <ScrollArea className="h-[400px] pr-4">
                        <div className="grid gap-4 py-4">
                            {availableVideos.map((v) => (
                                <div key={v.id} className="flex items-center justify-between">
                                    <Button
                                        onClick={() => handleVideoSelect(v)}
                                        className="justify-start h-auto p-2 flex-1"
                                        variant="ghost"
                                    >
                                        <div className="flex items-center space-x-4">
                                            <Image
                                                src={`data:image/jpeg;base64,${v.cover}`}
                                                alt={v.tittle}
                                                width={120}
                                                height={68}
                                                className="rounded object-cover"
                                            />
                                            <span className="truncate flex-1 text-left">{v.tittle}</span>
                                        </div>
                                    </Button>
                                    <Button
                                        onClick={() => handleVideoDelete(v.id)}
                                        className="ml-2"
                                        disabled={isDeleting}
                                        variant="destructive"
                                        size="sm"
                                    >
                                        {isDeleting ? (
                                            <span className="animate-spin">🔄</span>
                                        ) : (
                                            <span>🗑️</span>
                                        )}
                                    </Button>
                                </div>
                            ))}
                        </div>
                    </ScrollArea>
                </DialogContent>
            </Dialog>

            {deleteError && (
                <div className="mt-4 p-2 bg-red-100 border border-red-400 text-red-700 rounded">
                    Error: {deleteError}
                </div>
            )}

            {video ? (
  <Tabs defaultValue="details">
    <TabsList className="grid w-full grid-cols-1 mb-1">
      <TabsTrigger value="details">Details</TabsTrigger>
    </TabsList>
    <TabsContent value="details">
      <div className="space-y-4">
        {/* Privacy Settings */}
                  <div>
                    <Label  className="text-lg font-semibold">Title</Label>
                    <Input
                      id="title"
                      value={videoName}
                      onChange={(e) => setTitle(e.target.value)}
                      className="text-lg"
                      placeholder="Введіть назву відео"
                    />
                  </div>
        <div>
          <Label>Privacy Settings</Label>
          <RadioGroup value={visibility} onValueChange={setVisibility}>
            <div className="flex items-center space-x-2">
              <RadioGroupItem value="public" id="public" selectedValue={visibility} onValueChange={setVisibility}>
                Public
              </RadioGroupItem>
            </div>
            <div className="flex items-center space-x-2">
              <RadioGroupItem value="private" id="private" selectedValue={visibility} onValueChange={setVisibility}>
                Private
              </RadioGroupItem>
            </div>
            <div className="flex items-center space-x-2">
              <RadioGroupItem value="unlisted" id="unlisted" selectedValue={visibility} onValueChange={setVisibility}>
                Unlisted
              </RadioGroupItem>
            </div>
          </RadioGroup>
        </div>

        {/* Selected Privacy Setting */}
        <div>
          <Label>Selected Privacy Setting</Label>
          <p className="text-sm font-medium">{visibility.charAt(0).toUpperCase() + visibility.slice(1)}</p>
        </div>

        {/* Video Description */}
        <Textarea
          placeholder="Enter video description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          className="min-h-[200px]"
        />

<div className="space-y-4">
  <div>
    <h3 className="text-lg font-semibold">Choose a thumbnail</h3>
    <p>Set a thumbnail that stands out and draws viewers' attention.</p>
    <p className="text-sm text-gray-500">Recommended size is 1280x720</p>

    {/* Вибір файлу через прихований input */}
    <Button
      variant="outline"
      onClick={() => document.getElementById("thumbnailInput")?.click()}
    >
      Choose file
    </Button>

    <input
      id="thumbnailInput"
      type="file"
      accept="image/png, image/jpeg, image/jpg"
      className="hidden"
      onChange={handleThumbnailChange}
    />

    {/* Прев'ю мініатюри */}
    {thumbnailPreview && (
      <div className="mt-4">
        <p>Thumbnail preview:</p>
        <img
          src={thumbnailPreview as string}
          alt="Thumbnail Preview"
          className="thumbnail-preview"
          style={{ width: "300px", height: "auto" }}
        />
      </div>
    )}
</div>

          {/* Thumbnail Grid */}
          <div className="grid grid-cols-4 gap-4 mt-4">
            <img
              src={thumbnailPreview ? thumbnailPreview as string : "/placeholder.svg?height=720&width=1280"}
              alt="Selected Thumbnail"
              className="w-full rounded-lg"
            />
          </div>
        </div>

        {/* Video Details Section */}
        <div className="space-y-4">
          <h3 className="text-lg font-semibold">Video details</h3>
          <div className="space-y-2">
            {/* Video Category */}
            <Label>Video category</Label>
            <Select>
              <SelectTrigger onClick={() => setIsCategoryOpen(!isCategoryOpen)}>
                <SelectValue value={category} placeholder="Select a category" />
              </SelectTrigger>
              <SelectContent isOpen={isCategoryOpen}>
                {categories.map((categoryItem) => (
                  <SelectItem
                    key={categoryItem.id}
                    value={categoryItem.name}
                    onClick={() => {
                      setSelectedCategoryId(categoryItem.id);
                      setCategory(categoryItem.name);
                      setIsCategoryOpen(false);
                    }}
                  >
                    {categoryItem.name}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>

            {/* New Category */}
            <Input
              placeholder="New category"
              value={newCategory}
              onChange={(e) => setNewCategory(e.target.value)}
            />
            <Button type="button" onClick={addCategory}>
              <Plus className="h-4 w-4 mr-2" />
              Add
            </Button>
          </div>
        </div>

        {/* Age Restriction Section */}
        <div className="space-y-4">
          <h3 className="text-lg font-semibold mb-2">Age Restriction</h3>
          <p className="text-sm text-gray-600 mb-2">Would you like to restrict this video to viewers over 18 years old?</p>
          <p className="text-xs text-gray-500 mb-4">
            Age-restricted content may not appear in certain parts of the platform and could have limited advertising options. <a href="#" className="text-blue-500 hover:underline">Learn more</a>
          </p>
          <div className="space-y-2">
            <label className="flex items-center">
              <input
                type="radio"
                className="form-radio"
                name="ageRestriction"
                checked={isAgeRestricted}
                onChange={() => setAgeRestricted(true)}
              />
              <span className="ml-2">Yes, restrict to viewers over 18 only</span>
            </label>
            <label className="flex items-center">
              <input
                type="radio"
                className="form-radio"
                name="ageRestriction"
                checked={!isAgeRestricted}
                onChange={() => setAgeRestricted(false)}
              />
              <span className="ml-2">No, allow all viewers</span>
            </label>
          </div>

          {/* Copyright Status Section */}
          <h3 className="text-lg font-semibold mb-2">Copyright Status</h3>
          <p className="text-sm text-gray-600 mb-2">Does this video contain copyrighted material?</p>
          <p className="text-xs text-gray-500 mb-4">
            Ensure compliance with copyright regulations by declaring whether this content contains copyrighted materials. <a href="#" className="text-blue-500 hover:underline">Learn more</a>
          </p>
          <div className="space-y-2">
            <label className="flex items-center">
              <input
                type="radio"
                className="form-radio"
                name="copyrightStatus"
                checked={isCopyright}
                onChange={() => setIsCopyright(true)}
              />
              <span className="ml-2">Yes, this video contains copyrighted content</span>
            </label>
            <label className="flex items-center">
              <input
                type="radio"
                className="form-radio"
                name="copyrightStatus"
                checked={!isCopyright}
                onChange={() => setIsCopyright(false)}
              />
              <span className="ml-2">No, this video does not contain copyrighted content</span>
            </label>
          </div>
        </div>
      </div>
    </TabsContent>
  </Tabs>
) : null}

            {isUpdating && (
                <div className="mt-4">
                    <p>Updating video... {updateProgress}%</p>
                    <div className="w-full bg-gray-200 rounded-full h-2.5 dark:bg-gray-700">
                        <div 
                            className="bg-blue-600 h-2.5 rounded-full" 
                            style={{width: `${updateProgress}%`}}
                        ></div>
                    </div>
                </div>
            )}

            {updateComplete && (
                <div className="mt-4 text-green-600">
                    Update complete!
                </div>
            )}

            <Button onClick={handleSubmit} disabled={isUpdating} className="mt-6">
                {isUpdating ? 'Updating...' : 'Update Video'}
            </Button>
        </div>
    )
}

export default VideoUpdateInterface