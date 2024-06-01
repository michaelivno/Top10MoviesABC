import React, { useState, useEffect } from 'react';
import { TextField, Button, MenuItem, Typography, Paper, Box, Alert } from '@mui/material';
import { paperStyle, alertStyle, formBoxStyle } from '../style/movieFormStyle';

// This component handles the movie form for adding and updating movies
function MovieForm({ onMovieAdded, selectedMovie, onMovieUpdated, onCancelEdit }) {
  // State hooks for form inputs
  const [title, setTitle] = useState('');
  const [category, setCategory] = useState('');
  const [ranking, setRanking] = useState('');
  const [imageUrl, setImageUrl] = useState('');
  const [error, setError] = useState('');
  
  // List of categories - might add more later
  const categories = ['Action', 'Sci-Fi', 'Thriller', 'Comedy', 'Drama'];

  // Effect to set form values when a movie is selected for editing
  useEffect(() => {
    if (selectedMovie) {
      setTitle(selectedMovie.title);
      setCategory(selectedMovie.category);
      setRanking(selectedMovie.ranking);
      setImageUrl(selectedMovie.imageUrl || '');
    } else {
      resetForm(); // Reset form if no movie is selected
    }
  }, [selectedMovie]);

  // Function to handle form submission
  const handleSubmit = async (event) => {
    event.preventDefault();
    if (!title || !category || !ranking) {
      setError('All fields except Image URL are required.');
      return;
    }
    const newMovie = {
      title,
      category,
      ranking: parseInt(ranking, 10),
      imageUrl: imageUrl || '',
    };
    try {
      if (selectedMovie) {
        newMovie.id = selectedMovie.id;
        console.log("update", newMovie);
        await onMovieUpdated(selectedMovie.id, newMovie);
      } else {
        console.log("Add", newMovie);
        await onMovieAdded(newMovie);
      }
      resetForm(); // Clear the form after submission
    } catch (err) {
      setError(err.response?.data?.message || 'An error occurred');
    }
  };

  // Function to handle form reset
  const resetForm = () => {
    setTitle('');
    setCategory('');
    setRanking('');
    setImageUrl('');
    setError('');
  };

  // Function to handle cancel button click
  const handleCancel = () => {
    resetForm();
    if (onCancelEdit) {
      onCancelEdit(); // Notify parent to cancel edit mode
    }
  };

  // Function to handle changes in the ranking field
  const handleRankingChange = (e) => {
    const value = e.target.value;
    if (value === '' || /^\d+$/.test(value)) { // Ensure only non-negative integers are allowed
      setRanking(value);
    }
  };

  return (
    <Paper elevation={3} sx={paperStyle}>
      <Typography variant="h6" component="h2" gutterBottom>
        {selectedMovie ? 'Update Movie' : 'Add a New Movie'}
      </Typography>
      <Box component="form" onSubmit={handleSubmit}>
        <TextField
          label="Title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          fullWidth
          margin="normal"
          required
        />
        <TextField
          select
          label="Category"
          value={category}
          onChange={(e) => setCategory(e.target.value)}
          fullWidth
          margin="normal"
          required
        >
          {categories.map((option) => (
            <MenuItem key={option} value={option}>
              {option}
            </MenuItem>
          ))}
        </TextField>
        <TextField
          label="Rating"
          type="number"
          value={ranking}
          onChange={handleRankingChange}
          fullWidth
          margin="normal"
          required
        />
        <TextField
          label="Image URL"
          value={imageUrl}
          onChange={(e) => setImageUrl(e.target.value)}
          fullWidth
          margin="normal"
        />
        {error && (
          <Alert severity="error" sx={alertStyle}>
            {error}
          </Alert>
        )}
        <Box sx={formBoxStyle}>
          <Button type="submit" variant="contained" color="primary">
            {selectedMovie ? 'Update Movie' : 'Add Movie'}
          </Button>
          {selectedMovie && (
            <Button variant="outlined" color="secondary" onClick={handleCancel}>
              Cancel
            </Button>
          )}
        </Box>
      </Box>
    </Paper>
  );
}

export default MovieForm;
