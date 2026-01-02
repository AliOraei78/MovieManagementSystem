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

EF Core journey has officially begun!