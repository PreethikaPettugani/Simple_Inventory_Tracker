import React, { useEffect, useState } from 'react';
import { getItemsByCategory, getAllCategories } from '../Services/Api';
import { TextField, PrimaryButton } from '@fluentui/react';
import './Categories.css';

const Categories = () => {
  const [category, setCategory] = useState('');
  const [items, setItems] = useState([]);
  const [categories, setCategories] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 8;

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const data = await getAllCategories();
        setCategories(data);
      } catch (error) {
        console.error('Error fetching categories:', error);
      }
    };
    fetchCategories();
  }, []);

  const handleFetch = async (selectedCategory = null) => {
    const categoryToFetch = selectedCategory || category;
    try {
      const data = await getItemsByCategory(categoryToFetch);
      setItems(data);
      setCurrentPage(1);
    } catch (error) {
      console.error('Error fetching items by category:', error);
    }
  };

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = items.slice(indexOfFirstItem, indexOfLastItem);
  const totalPages = Math.ceil(items.length / itemsPerPage);

  return (
    <div className="categories-container">
      <h2 className="category-name">Categories</h2>

      <div className="search-container">
        <TextField
          className="category-search-box"
          placeholder="Category Name"
          value={category}
          onChange={(e, newValue) => setCategory(newValue)}
        />
        <PrimaryButton
          className="search-button"
          text="Search"
          onClick={() => handleFetch()}
        />
      </div>

      <div className="categories-buttons">
        <h3>Available Categories</h3>
        <div className="category-button-list">
          {categories.map((cat, index) => (
            <PrimaryButton
              key={index}
              text={cat}
              onClick={() => handleFetch(cat)}
              className="category-button"
            />
          ))}
        </div>
      </div>

      {items.length > 0 && (
        <>
          <div className="category-item-cards-container">
            {currentItems.map((item) => (
              <div key={item.itemId} className="category-item-card">
                <p><strong>Name:</strong> {item.name}</p>
                <p><strong>Description:</strong> {item.description}</p>
                <p><strong>Quantity:</strong> {item.quantity}</p>
                <p><strong>Category:</strong> {item.category}</p>
                <p><strong>Minimum Stock Threshold:</strong> {item.minimumStockThreshold}</p>
              </div>
            ))}
          </div>

          <div className="pagination-controls">
            <button
              onClick={() => setCurrentPage((prev) => Math.max(prev - 1, 1))}
              disabled={currentPage === 1}
              className={`pagination-button ${currentPage === 1 ? 'disabled' : ''}`}
            >
              Prev
            </button>
            <span className="pagination-text">Page {currentPage} of {totalPages}</span>
            <button
              onClick={() => setCurrentPage((prev) => Math.min(prev + 1, totalPages))}
              disabled={currentPage === totalPages}
              className={`pagination-button ${currentPage === totalPages ? 'disabled' : ''}`}
            >
              Next
            </button>
          </div>
        </>
      )}
    </div>
  );
};

export default Categories;

