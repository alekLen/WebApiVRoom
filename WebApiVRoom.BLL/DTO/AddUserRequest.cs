﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.BLL.DTO
{

    public class AddUserRequest : EventArgs
    {
        public User data { get; set; }
        public string type { get; set; }
        public EventAttributes event_attributes { get; set; }
        public class EventAttributes
        {
            public  class http_request {
                public string client_ip { get; set; }
                public string user_agent { get; set; }
            }
        }
        public class User
        {
            public bool? backup_code_enabled { get; set; }
            public bool? banned { get; set; }
            public bool? create_organization_enabled { get; set; }            
            public float? created_at { get; set; }
            public bool? delete_self_enabled { get; set; }
            public List<EmailAddress>? email_addresses { get; set; }
            public List<ExternalAccount>? external_accounts { get; set; }
            public string? external_id { get; set; }
            public string? first_name { get; set; }
            public bool? has_image { get; set; }
            public string id { get; set; }
            public string? image_url { get; set; }
            public float? last_active_at { get; set; }
            public string? last_name { get; set; }
            public float? last_sign_in_at { get; set; }
            public bool? locked { get; set; }
            public float? lockout_expires_in_seconds { get; set; }
            public float? mfa_disabled_at { get; set; }
            public float? mfa_enabled_at { get; set; }
            public List<string>? passkeys { get; set; }
            public bool? password_enabled { get; set; }
            public List<string>? phone_numbers { get; set; }
            public string? primary_email_address_id { get; set; }
            public string? primary_phone_number_id { get; set; } 
            public string? primary_web3_wallet_id { get; set; } 
            private string? private_metadata { get; set; }
            public string? profile_image_url { get; set; }
            public Metadata? public_metadata { get; set; }
            public List<string>? saml_accounts { get; set; }
            public bool? totp_enabled { get; set; }
            public bool? two_factor_enabled { get; set; }
            public Metadata? unsafe_metadata { get; set; }
            public float? updated_at { get; set; }
            public string? username { get; set; }
            public int? verification_attempts_remaining { get; set; }
            public List<string>? web3_wallets { get; set; }
        }
    }
    public class EmailAddress
    {
        public string email_address { get; set; }
        public string id { get; set; }
        public float created_at { get; set; }
        public List<object> linked_to { get; set; }
        public bool reserved { get; set; }
        public float updated_at { get; set; }
        public Verification verification { get; set; }
    }
    
    public class ExternalAccount
    {
        public string ApprovedScopes { get; set; }
        public long CreatedAt { get; set; }
        public string EmailAddress { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string GoogleId { get; set; }
        public string Id { get; set; }
        public string Label { get; set; }
        public string Object { get; set; }
        public string Picture { get; set; }
        public long UpdatedAt { get; set; }
        public string Username { get; set; }
        public Verification Verification { get; set; }
    }

    public class Verification { }

    public class Metadata { }

    public class Web3Wallet { }
 
}

