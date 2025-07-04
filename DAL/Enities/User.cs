﻿using System;
using System.Collections.Generic;

namespace DAL.Enities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
        
        // Store relationship
        public Guid? StoreId { get; set; }
        public virtual Store Store { get; set; }

        // Impersonation tracking
        public Guid? ImpersonatedByUserId { get; set; }
        public virtual User ImpersonatedByUser { get; set; }
        public DateTime? LastImpersonationDate { get; set; }

        // Audit fields
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string LastLoginIP { get; set; }

        // Navigation properties
        public virtual ICollection<User> ImpersonatedUsers { get; set; }
