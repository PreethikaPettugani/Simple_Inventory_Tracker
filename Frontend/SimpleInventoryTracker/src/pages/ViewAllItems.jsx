import React, { useEffect, useState } from 'react';
import { TextField, PrimaryButton, Dialog, DialogType, DialogFooter, DefaultButton } from '@fluentui/react';
import axios from 'axios';
import './ViewAllItems.css';
import { getItemByName } from '../Services/Api';

const ViewAllItems = () => {
  const [items, setItems] = useState([]);
  const [searchError, setSearchError] = useState('');
  const [isDialogVisible, setIsDialogVisible] = useState(false);
  const [searchName, setSearchName] = useState('');
  const [filteredItems, setFilteredItems] = useState([]);
  const [editItem, setEditItem] = useState(null);
  const [isDeleteSuccessDialogVisible, setIsDeleteSuccessDialogVisible] = useState(false);
  const [isAddDialogVisible, setIsAddDialogVisible] = useState(false);
  const [isEditDialogVisible, setIsEditDialogVisible] = useState(false);

  const [form, setForm] = useState({
    itemId: '',
    name: '',
    description: '',
    quantity: '',
    category: '',
    minimumStockThreshold: '',
  });

  const itemsPerPage = 8;
  const [currentPage, setCurrentPage] = useState(1);

  useEffect(() => {
    fetchItems();
  }, []);

  const fetchItems = async () => {
    try {
      const res = await axios.get('https://localhost:7045/api/Item');
      setItems(res.data);
      setFilteredItems(res.data);
    } catch (err) {
      console.error('Error fetching items:', err);
    }
  };

  const handleSearch = async () => {
    if (!searchName.trim()) return;

    try {
      const item = await getItemByName(searchName.trim());
      setFilteredItems([item]);
      setSearchError('');
      setIsDialogVisible(false);
      setCurrentPage(1);
    } catch (err) {
      setFilteredItems([]);
      setSearchError(err.message);
      setIsDialogVisible(true);
    }
  };

  const openAddDialog = () => {
    setForm({
      itemId: '',
      name: '',
      description: '',
      quantity: '',
      category: '',
      minimumStockThreshold: '',
    });
    setIsAddDialogVisible(true);
  };

  const openEditDialog = (item) => {
    setEditItem(item);
    setForm({
      itemId: item.itemId,
      name: item.name,
      description: item.description,
      quantity: item.quantity,
      category: item.category,
      minimumStockThreshold: item.minimumStockThreshold,
    });
    setIsEditDialogVisible(true);
  };

  const closeAddDialog = () => setIsAddDialogVisible(false);
  const closeEditDialog = () => {
    setIsEditDialogVisible(false);
    setEditItem(null);
  };

  const handleFormChange = (field, value) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleSaveAdd = async () => {
    try {
      await axios.post('https://localhost:7045/api/Item', form);
      fetchItems();
      closeAddDialog();
    } catch (err) {
      if (err.response && err.response.status === 500) {
        alert(`Item with ID ${form.itemId} already exists.`);
      } else {
        console.error('Error adding item:', err);
      }
    }
  };

  const handleSaveEdit = async () => {
    try {
      await axios.put(`https://localhost:7045/api/Item/${editItem.itemId}`, form);
      fetchItems();
      closeEditDialog();
    } catch (err) {
      console.error('Error updating item:', err);
    }
  };

  const handleDelete = async (id) => {
    try {
      await axios.delete(`https://localhost:7045/api/Item/${id}`);
      setIsDeleteSuccessDialogVisible(true);
    } catch (err) {
      console.error('Error deleting item:', err);
    }
  };

  const paginatedItems = filteredItems.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  const totalPages = Math.ceil(filteredItems.length / itemsPerPage);

  return (
    <div className="view-all-items-container">
      <h2 className="view-all-items-title">All Items</h2>
      <div className="search-bar">
        <TextField
          placeholder="Search by Item Name"
          value={searchName}
          onChange={(e, newValue) => setSearchName(newValue)}
        />
        <PrimaryButton text="Search" onClick={handleSearch} />
        <PrimaryButton text="Add Item" onClick={openAddDialog} />
      </div>

      <div className="item-cards-container">
        {paginatedItems.map((item) => (
          <div key={item.itemId} className="item-card">
            <p><strong>Name:</strong> {item.name}</p>
            <p><strong>Description:</strong> {item.description}</p>
            <p><strong>Quantity:</strong> {item.quantity}</p>
            <p><strong>Category:</strong> {item.category}</p>
            <p><strong>Minimum Stock Threshold:</strong> {item.minimumStockThreshold}</p>

            <div className="action-buttons">
              <button className="edit-button" onClick={() => openEditDialog(item)}>Edit</button>
              <button className="delete-button" onClick={() => handleDelete(item.itemId)}>Delete</button>
            </div>
          </div>
        ))}
      </div>

      {totalPages > 1 && (
        <div className="pagination-controls">
          <DefaultButton
            text="Prev"
            onClick={() => setCurrentPage((p) => Math.max(p - 1, 1))}
            disabled={currentPage === 1}
          />
          <span>Page {currentPage} of {totalPages}</span>
          <DefaultButton
            text="Next"
            onClick={() => setCurrentPage((p) => Math.min(p + 1, totalPages))}
            disabled={currentPage === totalPages}
          />
        </div>
      )}

      {/* add item */}
      <Dialog
        hidden={!isAddDialogVisible}
        onDismiss={closeAddDialog}
        dialogContentProps={{
          type: DialogType.largeHeader,
          title: 'Add New Item',
        }}
      >
        <TextField id="add-item-id" label="ID" value={form.itemId} onChange={(e, val) => handleFormChange('itemId', val)} />
        <TextField id="add-item-name" label="Name" value={form.name} onChange={(e, val) => handleFormChange('name', val)} />
        <TextField id="add-item-description" label="Description" value={form.description} onChange={(e, val) => handleFormChange('description', val)} />
        <TextField id="add-item-quantity" label="Quantity" type="number" value={form.quantity} onChange={(e, val) => handleFormChange('quantity', val)} />
        <TextField id="add-item-category" label="Category" value={form.category} onChange={(e, val) => handleFormChange('category', val)} />
        <TextField id="add-item-min-stock" label="Minimum Stock Threshold" type="number" value={form.minimumStockThreshold} onChange={(e, val) => handleFormChange('minimumStockThreshold', val)} />
        <DialogFooter>
          <PrimaryButton onClick={handleSaveAdd} text="Add" id="add-button" />
          <DefaultButton onClick={closeAddDialog} text="Cancel" />
        </DialogFooter>
      </Dialog>

      {/* edit item */}
      <Dialog
        hidden={!isEditDialogVisible}
        onDismiss={closeEditDialog}
        dialogContentProps={{
          type: DialogType.largeHeader,
          title: 'Edit Item',
        }}
      >
        <TextField id="edit-item-id" label="ID" value={form.itemId} onChange={(e, val) => handleFormChange('itemId', val)} disabled />
        <TextField id="edit-item-name" label="Name" value={form.name} onChange={(e, val) => handleFormChange('name', val)} />
        <TextField id="edit-item-description" label="Description" value={form.description} onChange={(e, val) => handleFormChange('description', val)} />
        <TextField id="edit-item-quantity" label="Quantity" type="number" value={form.quantity} onChange={(e, val) => handleFormChange('quantity', val)} />
        <TextField id="edit-item-category" label="Category" value={form.category} onChange={(e, val) => handleFormChange('category', val)} />
        <TextField id="edit-item-min-stock" label="Minimum Stock Threshold" type="number" value={form.minimumStockThreshold} onChange={(e, val) => handleFormChange('minimumStockThreshold', val)} />
        <DialogFooter>
          <PrimaryButton onClick={handleSaveEdit} text="Update" id="edit-update-button" />
          <DefaultButton onClick={closeEditDialog} text="Cancel" id="eidt-cancel-button" />
        </DialogFooter>
      </Dialog>

      {/* search */}
      <Dialog
        hidden={!isDialogVisible}
        onDismiss={() => {
          setIsDialogVisible(false);
          setSearchName('');
          fetchItems();
        }}
        dialogContentProps={{
          type: DialogType.normal,
          title: 'Item Not Found',
          subText: `Item '${searchName}' not found.`
        }}
      >
        <DialogFooter>
          <PrimaryButton
            onClick={() => {
              setIsDialogVisible(false);
              setSearchName('');
              fetchItems();
            }}
            text="OK"
          />
        </DialogFooter>
      </Dialog>


      <Dialog
        hidden={!isDeleteSuccessDialogVisible}
        onDismiss={() => {
          setIsDeleteSuccessDialogVisible(false);
          fetchItems();
        }}
        dialogContentProps={{
          type: DialogType.normal,
          title: 'Item Deleted',
          subText: 'The item has been deleted successfully.',
        }}
      >
        <DialogFooter>
          <PrimaryButton
            id="delete-ok-button"
            onClick={() => {
              setIsDeleteSuccessDialogVisible(false);
              fetchItems();
            }}
            text="OK"
          />
        </DialogFooter>
      </Dialog>
    </div>
  );
};

export default ViewAllItems;

