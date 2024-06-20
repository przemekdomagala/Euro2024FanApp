using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Euro2024REST.Models;

namespace Euro2024REST.Data;

    public class ContextDb : DbContext
    {
        public ContextDb (DbContextOptions<ContextDb> options)
            : base(options)
        {
        }

        public DbSet<Euro2024REST.Models.Match> Match { get; set; } = default!;
        public DbSet<Euro2024REST.Models.Player> Player { get; set; } = default!;
        public DbSet<Euro2024REST.Models.Team> Team { get; set; } = default!;

public DbSet<Euro2024REST.Models.User> User { get; set; } = default!;

public DbSet<Euro2024REST.Models.Predictions> Predictions { get; set; } = default!;
    }
