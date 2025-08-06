import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import ViewAllItems from '../pages/ViewAllItems';
import axios from 'axios';

jest.mock('axios');

describe('ViewAllItems Component', () => {
  beforeEach(() => {
    axios.get.mockResolvedValue({ data: [] }); 
  });

  test('renders title and buttons', async () => {
    render(<ViewAllItems />);
    expect(await screen.findByText('All Items')).toBeInTheDocument();
    expect(screen.getByText('Add Item')).toBeInTheDocument();
    expect(screen.getByText('Search')).toBeInTheDocument();
  });

  test('opens Add Item dialog when Add Item button clicked', async () => {
    render(<ViewAllItems />);
    fireEvent.click(screen.getByText('Add Item'));
    expect(await screen.findByText('Add New Item')).toBeInTheDocument();
  });

  test('fills and submits add form', async () => {
    axios.post.mockResolvedValueOnce({});
    render(<ViewAllItems />);

    fireEvent.click(screen.getByText('Add Item'));

    fireEvent.change(screen.getByLabelText('ID'), { target: { value: '101' } });
    fireEvent.change(screen.getByLabelText('Name'), { target: { value: 'Test Item' } });
    fireEvent.change(screen.getByLabelText('Description'), { target: { value: 'Description' } });
    fireEvent.change(screen.getByLabelText('Quantity'), { target: { value: '20' } });
    fireEvent.change(screen.getByLabelText('Category'), { target: { value: 'Category A' } });
    fireEvent.change(screen.getByLabelText('Minimum Stock Threshold'), { target: { value: '5' } });

    fireEvent.click(screen.getByText('Add'));

    await waitFor(() => {
      expect(axios.post).toHaveBeenCalledWith(
        'https://localhost:7045/api/Item',
        expect.objectContaining({
          itemId: '101',
          name: 'Test Item',
        })
      );
    });
  });

  test('opens edit dialog and updates item', async () => {
    const mockItems = [
      {
        itemId: '102',
        name: 'Old Item',
        description: 'Old desc',
        quantity: 15,
        category: 'Old Cat',
        minimumStockThreshold: 3,
      },
    ];

    axios.get.mockResolvedValueOnce({ data: mockItems });
    axios.put.mockResolvedValueOnce({});

    render(<ViewAllItems />);
    await screen.findByText('Old Item');

    fireEvent.click(screen.getByText('Edit'));

    expect(screen.getByText('Edit Item')).toBeInTheDocument();

    fireEvent.change(screen.getByLabelText('Name'), { target: { value: 'Updated Item' } });
    fireEvent.click(screen.getByText('Update'));

    await waitFor(() => {
      expect(axios.put).toHaveBeenCalledWith(
        'https://localhost:7045/api/Item/102',
        expect.objectContaining({
          name: 'Updated Item',
        })
      );
    });
  });

  test('deletes an item when Delete button is clicked', async () => {
  const mockItems = [
    {
      itemId: '103',
      name: 'Delete Me',
      description: 'To be deleted',
      quantity: 10,
      category: 'Test',
      minimumStockThreshold: 2,
    },
  ];

  
  axios.get.mockResolvedValueOnce({ data: mockItems });

  axios.delete.mockResolvedValueOnce({});

  axios.get.mockResolvedValue({ data: [] });

  render(<ViewAllItems />);
  expect(await screen.findByText('Delete Me')).toBeInTheDocument();

  fireEvent.click(screen.getByText('Delete'));

  await waitFor(() => {
    expect(axios.delete).toHaveBeenCalledWith('https://localhost:7045/api/Item/103');
  });

  expect(await screen.findByText('Item Deleted')).toBeInTheDocument();
 
  fireEvent.click(screen.getByText('OK'));

  await waitFor(() => {
    expect(screen.queryByText('Delete Me')).not.toBeInTheDocument();
  });
});
});
