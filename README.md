## Project Setup - Movie Management System

**Objective:**  
Create a brand-new, clean solution for the Entity Framework Core phase, completely separate from previous projects. The new system will be a professional **Movie Management System**.

**Project Name:** `MovieManagementSystem`

**Domain Overview:**  
A full-featured movie catalog and management application including:
- Movies
- Genres
- Actors
- Directors
- Studios
- User reviews and ratings
- Watchlists and favorites

## Day 1: EF Core Basics – DbContext, Code First, First Migration

**Completed Today:**
- Created initial entities: Movie, Genre, Actor
- Built base AppDbContext with DbSets
- Configured PostgreSQL connection via appsettings.json
- Implemented basic seed data in OnModelCreating
- Generated and applied first Code-First migration
- Verified database schema and seed data in pgAdmin

**Database Status:**
- Database: `MovieDb`
- Tables created: Movies, Genres, Actors
- Initial seed data inserted (2 movies, 4 genres)

## Day 2: Advanced Modeling – Relationships (1-1, 1-M, M-M)

**Completed Today:**
- Added new entities: Studio, MovieDetail
- Implemented real-world relationships:
  - One-to-Many: Movie → Studio (with SetNull on delete)
  - Many-to-Many: Movie ↔ Genre (junction table MovieGenres)
  - Many-to-Many: Movie ↔ Actor (junction table MovieActors)
  - One-to-One: Movie → MovieDetail (shared primary key)
- Configured all relationships using Fluent API
- Updated seed data to include relationships
- Generated and applied new migration
- Verified schema and data in PostgreSQL

**Database Updates:**
- New tables: Studios, MovieGenres, MovieActors, MovieDetails
- Proper foreign keys and cascade behaviors
- Seed data with connected entities

## Day 3 - Phase 4: Full Fluent API Configuration

**Completed Today:**
- Removed all Data Annotations from entities
- Configured every model property using Fluent API:
  - Keys, Required, MaxLength, Precision
  - Indexes (single and composite)
  - Unique constraints
  - Concurrency Token (RowVersion)
  - Column types (date, decimal)
- Relationships fully configured in Fluent API (no conventions)
- Owned-like configuration for MovieDetail
- New migration applied with all Fluent API changes
- Verified schema constraints and indexes in PostgreSQL

**Key Learnings:**
- Fluent API gives complete control over database schema
- Better for complex configurations and production applications
- Indexes and constraints defined explicitly
- Concurrency token ready for optimistic locking

## Day 4: Inheritance Strategies (TPH, TPT, TPC)

**Completed Today:**
- Created hierarchical model: Person → Member → Librarian
- Implemented all three EF Core inheritance strategies:
  - TPH (Table Per Hierarchy) - single table with discriminator (default)
  - TPT (Table Per Type) - base table + separate tables for derived types
  - TPC (Table Per Concrete Type) - separate full table for each concrete class
- Configured mapping strategies using Fluent API (.NET 8+)
- Generated and applied migration for selected strategy
- Analyzed resulting database schema in PostgreSQL

**Key Learnings:**
- TPH: Best performance, simplest queries, but can lead to sparse tables
- TPT: Better normalization, cleaner data, but requires JOINs
- TPC: Fast reads, no JOINs, but data duplication and complex writes
- Choice depends on query patterns and data access needs

## Day 5: Advanced Migrations

**Completed Today:**
- Complex seed data with relationships (Many-to-Many via HasData)
- Custom SQL in migrations (indexes, functions, triggers)
- Created trigram index for fuzzy search
- Implemented PostgreSQL function and trigger for rating average
- Idempotent SQL with IF NOT EXISTS / CREATE OR REPLACE
- Migration rollback testing
- Production migration best practices (script generation, dry-run)

**Key Learnings:**
- HasData for complex relationships (junction tables)
- Raw SQL for PostgreSQL-specific features
- Triggers and functions in migrations
- Safe rollback and production deployment strategies
- Generating SQL scripts for DBA review