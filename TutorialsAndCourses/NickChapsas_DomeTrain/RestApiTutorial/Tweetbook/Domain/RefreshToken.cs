﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Tweetbook.Domain;

public class RefreshToken
{
    [Key] public Guid Token { get; set; }

    public string JwtId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool Used { get; set; }

    public bool Invalidated { get; set; }
    public string UserId { get; set; }

    [ForeignKey(nameof(UserId))] public IdentityUser User { get; set; }
}