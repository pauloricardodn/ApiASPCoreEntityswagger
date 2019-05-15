﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OSEventos.Repository.Extensions
{
    public abstract class EntityTypeConfiguration<TEntity> where TEntity : class
    {
        public abstract void Map(EntityTypeBuilder<TEntity> builder);
    }
}
