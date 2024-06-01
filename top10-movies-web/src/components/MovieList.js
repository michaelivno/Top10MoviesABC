import React, { useEffect, useState } from 'react';
import { Container, Typography, List, ListItem, ListItemText, ListItemAvatar, Avatar, Divider, Box, Paper, Button, MenuItem, TextField, Snackbar, Alert, CircularProgress } from '@mui/material';
import { getMovies, addMovie, updateMovie } from '../services/movieService';
import MovieDetail from './MovieDetail';
import MovieForm from './MovieForm';
import AlertTitle from '@mui/material/AlertTitle';
import { containerStyle, paperStyle, boxStyle, listItemStyle, listItemTextStyle, alertStyle } from '../style/movieListStyle';
import '../style/style.css'; // Import the external CSS file

// Main component for displaying the movie list
function MovieList() {
  const [movies, setMovies] = useState([]); // State to hold the list of movies
  const [filteredMovies, setFilteredMovies] = useState([]); // State to hold the filtered list of movies
  const [selectedMovie, setSelectedMovie] = useState(null); // State to hold the selected movie
  const [editingMovie, setEditingMovie] = useState(null); // State to hold the movie being edited
  const [category, setCategory] = useState(''); // State to hold the selected category for filtering
  const [error, setError] = useState(''); // State to hold any error messages
  const [success, setSuccess] = useState(''); // State to hold success messages
  const [loading, setLoading] = useState(true); // State to show loading spinner

  // Effect to load movies when the component mounts
  useEffect(() => {
    loadMovies();
  }, []);

  // Function to load movies from the service
  const loadMovies = async () => {
    try {
      setLoading(true); // Start loading
      const movies = await getMovies(); // Fetch movies
      setMovies(movies); // Set movies state
      setFilteredMovies(movies); // Set filtered movies state
      setLoading(false); // End loading
    } catch (error) {
      console.error('Error fetching movies:', error); // Log error
      setLoading(false); // End loading
      setError('An error occurred while fetching movies.'); // Set error state
    }
  };

  // Function to handle movie selection
  const handleMovieClick = (movie) => {
    setSelectedMovie(movie);
  };

  // Function to handle editing movie
  const handleEditClick = (movie) => {
    setEditingMovie(movie);
  };

  // Function to handle adding a new movie
  const handleMovieAdded = async (newMovie) => {
    try {
        // Check if the same movie already exists
        const existingMovie = movies.find(movie => movie.title === newMovie.title && movie.category === newMovie.category);
        if (existingMovie) {
            setError('A movie with the same title and category already exists.');
            return;
        }

        const updatedMovies = await addMovie(newMovie);
        if (Array.isArray(updatedMovies)) {
            if (updatedMovies.length > 0) {
                setMovies(updatedMovies);
                setFilteredMovies(updatedMovies);
                setError(''); 
            } else {
                setError('New movie ranking is not high enough to be added to the top 10 list.');
            }
        } else {
            setError('There is a problem now, try again later.....');
        }
    } catch (error) {
        console.error('Error adding movie:', error);
        setError(error.response?.data?.message || 'There is a problem now, try again later.....');
    }
};


  // Function to handle updating an existing movie
  const handleMovieUpdated = async (id, updatedMovie) => {
    try {
      const updatedMovies = await updateMovie(id, updatedMovie);
      if (updatedMovies) {
        setMovies(updatedMovies); // Update movies state
        setFilteredMovies(updatedMovies); // Update filtered movies state
        setEditingMovie(null); // Reset editing movie state
        setSelectedMovie(null); // Reset selected movie state
        setSuccess('Movie updated successfully!'); // Set success message
      } else {
        setError('An error occurred while updating the movie.'); // Set error message
      }
    } catch (error) {
      console.error('Error updating movie:', error); // Log error
      setError(error.response?.data?.message || 'An error occurred while updating the movie.');
    }
  };

  // Function to handle category change for filtering
  const handleCategoryChange = (event) => {
    const selectedCategory = event.target.value;
    setCategory(selectedCategory); // Update category state
    if (selectedCategory) {
      setFilteredMovies(movies.filter(movie => movie.category === selectedCategory)); // Filter movies by category
    } else {
      setFilteredMovies(movies); // Reset filtered movies
    }
  };

  // Function to close error message
  const handleCloseError = () => {
    setError('');
  };

  // Function to close success message
  const handleCloseSuccess = () => {
    setSuccess('');
  };

  // Function to cancel editing
  const handleCancelEdit = () => {
    setEditingMovie(null);
  };

  return (
    <Container sx={containerStyle}>
     <Box className="styled-header">
      <Typography variant="h4" component="h1" gutterBottom className="styled-header-text">
        ABC Company : Top 10 Movies
      </Typography>
    </Box>
      <TextField
        select
        label="Filter by Category"
        value={category}
        onChange={handleCategoryChange}
        fullWidth
        margin="normal"
      >
        <MenuItem value="">All</MenuItem>
        <MenuItem value="Action">Action</MenuItem>
        <MenuItem value="Sci-Fi">Sci-Fi</MenuItem>
        <MenuItem value="Thriller">Thriller</MenuItem>
        <MenuItem value="Comedy">Comedy</MenuItem>
        <MenuItem value="Drama">Drama</MenuItem>
      </TextField>
      <Box display="flex" flexDirection={{ xs: 'column', sm: 'row' }} gap={4}>
        <Paper elevation={3} sx={paperStyle}>
          {loading ? (
            <Box sx={boxStyle}>
              <CircularProgress />
            </Box>
          ) : (
            <List>
              {filteredMovies.sort((a, b) => b.ranking - a.ranking).map(movie => (
                <div key={movie.id}>
                  <ListItem onClick={() => handleMovieClick(movie)} sx={listItemStyle}>
                    <Box display="flex" alignItems="center">
                      <ListItemAvatar>
                        <Avatar
                          variant="square"
                          src={movie.imageUrl}
                          alt={movie.title}
                          onError={(e) => e.target.style.display = 'none'}
                        />
                      </ListItemAvatar>
                      <ListItemText primary={movie.title} secondary={`Rating: ${movie.ranking}`} sx={listItemTextStyle} />
                    </Box>
                    <Button variant="contained" color="primary" onClick={() => handleEditClick(movie)}>
                      Edit
                    </Button>
                  </ListItem>
                  <Divider component="li" />
                </div>
              ))}
            </List>
          )}
        </Paper>
        <Box flex={1}>
          <MovieForm
            onMovieAdded={handleMovieAdded}
            selectedMovie={editingMovie}
            onMovieUpdated={handleMovieUpdated}
            onCancelEdit={handleCancelEdit}
          />
        </Box>
      </Box>
      <Snackbar open={!!error} autoHideDuration={3000} onClose={handleCloseError} anchorOrigin={{ vertical: 'top', horizontal: 'center' }}>
        <Alert onClose={handleCloseError} variant="filled" severity="error" sx={alertStyle}>
          <AlertTitle>Error</AlertTitle>
          {error}
        </Alert>
      </Snackbar>
      <Snackbar open={!!success} autoHideDuration={3000} onClose={handleCloseSuccess} anchorOrigin={{ vertical: 'top', horizontal: 'center' }}>
        <Alert onClose={handleCloseSuccess} variant="filled" severity="success" sx={alertStyle}>
          <AlertTitle>Success</AlertTitle>
          {success}
        </Alert>
      </Snackbar>
      {selectedMovie && !editingMovie && <MovieDetail movie={selectedMovie} onClose={() => setSelectedMovie(null)} />}
    </Container>
  );
}

export default MovieList;
