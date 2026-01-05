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

## Day 6: Change Tracker Deep Dive

**Completed Today:**
- Explored all entity states: Added, Modified, Unchanged, Deleted, Detached
- AsNoTracking for read-only queries (performance gain)
- Attach vs Update vs manual property marking
- Preventing over-posting in API updates
- Explicit state management with Entry().State
- ChangeTracker.Clear() for batch operations
- DetectChanges and tracking diagnostics

**Key Learnings:**
- AsNoTracking is essential for GET endpoints
- Never use Update() on untrusted input (over-posting risk)
- Prefer property-level IsModified for secure updates
- Change Tracker is powerful but can hurt performance if not managed
- Use Clear() or AsNoTracking for large read operations

## Day 7: Advanced Querying

**Completed Today:**
- Eager Loading with Include and ThenInclude
- Explicit Loading for on-demand related data
- Projection with Select to prevent over-fetching and N+1
- Split Queries for better performance with complex includes
- Demonstration of Lazy Loading risks (N+1 problem)
- Best practices:
  - Prefer Projection over full entity loading in APIs
  - Use AsSplitQuery for many includes
  - Avoid Lazy Loading in web applications

**Key Learnings:**
- Eager: Load everything upfront (good for known needs)
- Explicit: Load when needed (fine-grained control)
- Projection: Best for APIs (minimal data transfer)
- Split Query: Prevents performance issues with multiple collections

## Day 8: Performance Tuning

**Completed Today:**
- Enabled EF Core detailed logging to console
- AsNoTracking for read-only queries (memory & speed optimization)
- Compiled Queries for frequently executed parameterized queries
- Batch Update/Delete with ExecuteUpdateAsync/ExecuteDeleteAsync
- Indexing at EF Core level (single, composite, unique, filtered)
- Performance best practices applied

**Key Learnings:**
- AsNoTracking is mandatory for GET endpoints
- Compiled Queries significantly speed up repeated queries
- Batch operations reduce round-trips to database
- Indexes should be defined in Fluent API for control
- Logging helps identify slow queries

## Day 9: Global Query Filters + Soft Delete + Multi-Tenancy

**Completed Today:**
- Added ISoftDelete and IHasTenant interfaces
- Implemented Soft Delete (IsDeleted + DeletedAt) on core entities
- Implemented Multi-Tenancy (TenantId) on core entities
- Global Query Filters for:
  - Automatic soft delete filtering
  - Tenant isolation (data per tenant)
- CurrentTenant service for runtime tenant resolution
- Tested soft delete and tenant filtering in API

**Key Learnings:**
- Global Query Filters are powerful for cross-cutting concerns
- Soft Delete prevents data loss and simplifies recovery
- Multi-Tenancy with filters ensures data isolation
- Combine filters for secure, clean data access
- Use scoped service for current tenant in real apps

## Day 10: Advanced Transactions + SaveChanges Interceptor

**Completed Today:**
- Created AuditLog entity for tracking changes
- Implemented SaveChangesInterceptor for automatic auditing
- Captured Added/Modified/Deleted operations
- Stored changes as JSON with entity name, ID, user, and timestamp
- Registered interceptor in DI container
- Tested full audit trail on movie operations

**Key Learnings:**
- Interceptors run before/after SaveChanges
- ChangeTracker.Entries() gives full access to changes
- JsonSerializer for flexible change storage
- Perfect for audit trails, logging, or soft-delete automation
- External transactions can be managed separately

## Day 11 - Phase 4: EF Core Extensions

**Completed Today:**
- Installed and configured three powerful EF Core extensions:
  - EntityFramework.Exceptions: Human-readable database exceptions (Unique, FK, etc.)
  - Z.EntityFramework.Extensions: High-performance Bulk Insert/Update/Delete
  - EF+ (Entity Framework Plus): Query caching and advanced auditing
- Demonstrated:
  - Bulk operations with 1000+ records in milliseconds
  - Clean exception handling for PostgreSQL errors
  - Query caching with FromCacheAsync
- Best practices for using extensions in production

**Key Learnings:**
- Exceptions package makes debugging database errors easy
- Bulk extensions are essential for large data operations
- Query cache improves performance for repeated queries
- Extensions should be used carefully (licensing, compatibility)