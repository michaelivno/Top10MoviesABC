# ABC Company : Top 10 Movies

Welcome to the **ABC Company: Top 10 Movies** project! 🎬🍿 This repository contains both the front-end and back-end components of a web application that showcases the top 10 movies. The project is built using React for the front-end and ASP.NET Core with SQLite for the back-end. 

## Table of Contents
1. [Overview](#overview)
2. [Features](#features)
3. [Tech Stack](#tech-stack)
4. [Installation](#installation)
5. [Running the Project](#running-the-project)
6. [Notes](#notes)

## Overview

This project aims to provide a seamless experience for managing and viewing a list of top 10 movies. It allows users to add new movies, update existing ones, and view details of each movie in a stylish and elegant interface. The list is dynamically sorted based on movie rankings, ensuring only the top 10 movies are displayed at any time. (P.S. It’s pretty cool!)

## Features

### Front-end
- Built with React and Material-UI for a modern, responsive interface.
- Stylish header and list views with elegant gradients and shadows.
- Movie details displayed in a modal with the title centered over the image.
- Form for adding and editing movies with validation and error handling.

### Back-end
- Built with ASP.NET Core 8 and SQLite for simplicity and efficiency.
- Dapper for data access – because who needs the overhead, right?
- AutoMapper for easy mapping between DTOs and entities.
- Centralized error handling.
- Logging with NLog – because logs are the windows into the soul of an app.

## Tech Stack

### Front-end
- React-Hooks
- Material-UI
- Axios (for making HTTP requests)

### Back-end
- ASP.NET Core 8
- SQLite
- Dapper
- AutoMapper
- NLog

## Installation

Alrighty, let's get this party started! Follow these steps to get the project up and running.

### Prerequisites
- Node.js (version 18 or higher)
- .NET SDK (version 8)
- A sense of adventure 🚀

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/michaelivno/Top10MoviesABC.git
   cd top10movies
   ```

2. **Install Front-end Dependencies**
   Navigate to the front-end directory and install the npm packages.
   ```bash
   cd top10-movies-web
   npm install
   ```

3. **Install Back-end Dependencies**
   Navigate to the back-end directory and install the NuGet packages.
   ```bash
   cd Top10MoviesApi
   dotnet restore
   ```

## Running the Project

Let's make this thing fly! Here's how you do it.

### Running the Back-end

1. Navigate to the Backend directory:
   ```bash
   cd Top10MoviesApi
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Run the project:
   ```bash
   dotnet run
   ```

### Running the Front-end

1. Navigate to the Frontend directory:
   ```bash
   cd top10-movies-web
   ```

2. Start the development server:
   ```bash
   npm start
   ```

### Notes

1. The back-end server will start on `http://localhost:5178` and the front-end server will start on `http://localhost:3000`.
2. Make sure to configure CORS properly if you’re working in a development environment.

## Conclusion

Well, that's about it! You've got yourself a shiny new Top 10 Movies app, complete with a snazzy UI and a rock-solid back-end. Feel free to play around, and make it your own. And remember, if anything goes wrong, you can always blame me Michael Ivno. Have A Nice One! 😉

Enjoy and happy coding! 🎉
