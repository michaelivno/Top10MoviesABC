import axios from 'axios';

// Base URL for the API endpoint
const API_URL = 'http://localhost:5178/api/movies';

// Function to fetch all movies
export const getMovies = async () => {
  const response = await axios.get(API_URL);
  return response.data; // Return the data from the response
};

// Function to add a new movie
export const addMovie = async (movie) => {
  try {
    const response = await axios.post(API_URL, movie);
    return response.data; // Should return the updated movies array
  } catch (error) {
    // Log the error if adding a movie fails
    console.error('Error adding movie:', error.response?.data?.message || error.message);
    return null; // Return null to indicate failure
  }
};

// Function to update an existing movie
export const updateMovie = async (id, movie) => {
  try {
    const response = await axios.put(`${API_URL}/${id}`, movie);
    return response.data; // Should return the updated movies array
  } catch (error) {
    // Log the error if updating a movie fails
    console.error('Error updating movie:', error.response?.data?.message || error.message);
    return null; // Return null to indicate failure
  }
};
