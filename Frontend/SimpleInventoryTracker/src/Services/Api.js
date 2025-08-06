const BASE_URL = 'https://localhost:7045/api';

export async function getAllItems() {
  const response = await fetch(`${BASE_URL}/Item`);
  if (!response.ok) {
    throw new Error('Failed to fetch items');
  }
  return await response.json();
}

export async function getItemByName(name) {
  const response = await fetch(`${BASE_URL}/Item/byname/${encodeURIComponent(name)}`);
  const contentType = response.headers.get('content-type');

  if (response.ok) {
    // If item is found, return the JSON data
    if (contentType && contentType.includes('application/json')) {
      return await response.json();
    }
    // If content is plain text, treat it as "not found"
    const message = await response.text();
    throw new Error(message);
  }
  // For other non-200 errors
  const errorText = await response.text();
  throw new Error(errorText || 'Failed to fetch item');
}

export async function createItem(item) {
  const response = await fetch(`${BASE_URL}/Item`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(item)
  });
  if (!response.ok) {
    throw new Error('Failed to create item');
  }
  return await response.json();
}

export async function updateItem(id, updatedItem) {
  const response = await fetch(`/api/Item/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(updatedItem)
  });
  if (!response.ok) throw new Error('Failed to update item');
}

export async function deleteItem(id) {
  const response = await fetch(`/api/Item/${id}`, {
    method: 'DELETE'
  });
  if (!response.ok) throw new Error('Failed to delete item');
}

export async function getLowStockItems() {
  const response = await fetch(`${BASE_URL}/Item/lowstock`);
  if (!response.ok) {
    throw new Error('Failed to fetch low stock items');
  }
  return await response.json();
}

export async function getHighStockItems() {
  const response = await fetch(`${BASE_URL}/Item/highstock`);
  if (!response.ok) {
    throw new Error('Failed to fetch high stock items');
  }
  return await response.json();
}

export async function getItemsByCategory(categoryName) {
  const response = await fetch(`${BASE_URL}/Item/category?name=${encodeURIComponent(categoryName)}`);
  if (!response.ok) {
    throw new Error(`Failed to fetch items in category: ${categoryName}`);
  }
  return await response.json();
}

export async function getAllCategories() {
  const response = await fetch(`${BASE_URL}/Item/allCategories`);
  if (!response.ok) {
    throw new Error('Failed to fetch categories');
  }
  return await response.json();
}

export async function updateItemQuantity(id, quantity) {
  const response = await fetch(`${BASE_URL}/Item/${id}/quantity`, {
    method: 'PATCH',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ quantity }),
  });
  if (!response.ok) {
    throw new Error('Failed to update item quantity');
  }

  return await response.json();
}
