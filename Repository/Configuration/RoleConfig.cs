﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Repository.Configuration;
public class RoleConfig : IEntityTypeConfiguration<IdentityRole> {
    public void Configure(EntityTypeBuilder<IdentityRole> builder) {
        builder.HasData(
            new IdentityRole {
                Name = "Manager",
                NormalizedName = "MANAGER"
            },
            new IdentityRole {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            });
    }
}