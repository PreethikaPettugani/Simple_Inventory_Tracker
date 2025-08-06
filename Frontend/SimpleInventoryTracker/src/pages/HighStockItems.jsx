import React, { useEffect, useState } from 'react';
import { getHighStockItems, updateItemQuantity } from '../Services/Api';
import './HighStockItems.css';
import { DefaultButton, PrimaryButton } from '@fluentui/react';

function HighStockItems() {
  const [items, setItems] = useState([]);
  const [editItem, setEditItem] = useState(null);
  const [newQuantity, setNewQuantity] = useState('');
  const [currentPage, setCurrentPage] = useState(1);

  const itemsPerPage = 8;

  useEffect(() => {
    fetchHighStock();
  }, []);

  const fetchHighStock = async () => {
    try {
      const data = await getHighStockItems();
      setItems(data);
    } catch (error) {
      console.error('Error fetching high stock items:', error);
    }
  };

  const openEdit = (item) => {
    setEditItem(item);
    setNewQuantity('');
  };

  const closeEdit = () => {
    setEditItem(null);
    setNewQuantity('');
  };

  const handleSaveQuantity = async () => {
    try {
      const updated = await updateItemQuantity(editItem.itemId, parseInt(newQuantity, 10));
      console.log('Updated:', updated);
      await fetchHighStock();
      closeEdit();
    } catch (err) {
      console.error('Error updating quantity:', err);
    }
  };


  const totalPages = Math.ceil(items.length / itemsPerPage);
  const paginatedItems = items.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage);

  return (
    <div className="Highstock-container">
      <h2 className="Highstock-heading">High Stock Items</h2>

      <div className="Highstock-card-container">
        {paginatedItems.length === 0 ? (
          <p>No high stock items found.</p>
        ) : (
          paginatedItems.map((item) => (
            <div key={item.itemId} className="Highstock-card">
              <p><strong>Name:</strong> {item.name}</p>
              <p><strong>Description:</strong> {item.description}</p>
              <p><strong>Quantity:</strong> {item.quantity}</p>
              <p><strong>Category:</strong> {item.category}</p>
              <p><strong>Min Stock Threshold:</strong> {item.minimumStockThreshold}</p>

              <div className="Highstock-action-buttons">
                <button className="Highstock-edit-button" onClick={() => openEdit(item)}>Edit</button>
              </div>
            </div>
          ))
        )}
      </div>

      {totalPages > 1 && (
        <div className="Highstock-pagination-controls">
          <DefaultButton
            text="Prev"
            onClick={() => setCurrentPage((prev) => Math.max(prev - 1, 1))}
            disabled={currentPage === 1}
          />
          <span style={{ margin: '0 10px' }}>
            Page {currentPage} of {totalPages}
          </span>
          <DefaultButton
            text="Next"
            onClick={() => setCurrentPage((prev) => Math.min(prev + 1, totalPages))}
            disabled={currentPage === totalPages}
          />
        </div>
      )}

      {editItem && (
        <div className="edit-overlay">
          <div className="edit-modal">
            <h3>Edit Quantity</h3>
            <p><strong>ID:</strong> {editItem.itemId}</p>
            <p><strong>Name:</strong> {editItem.name}</p>
            <label>New Quantity:</label>
            <input
              id="newQuantityInput"
              type="number"
              value={newQuantity}
              onChange={(e) => setNewQuantity(e.target.value)}
              className="modal-quantity-input"
            />
            <div className="modal-buttons">
              <PrimaryButton text="Update" onClick={handleSaveQuantity} id="edit-update-button" />
              <DefaultButton text="Cancel" onClick={closeEdit} />
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default HighStockItems;

