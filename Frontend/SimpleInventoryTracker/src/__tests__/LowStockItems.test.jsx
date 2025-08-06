import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import LowStockItems from '../pages/LowStockItems';
import { getLowStockItems, updateItemQuantity } from '../Services/Api';

jest.mock('../Services/Api');

describe('LowStockItems Component', () => {
  const mockItems = [
    {
      itemId: 1,
      name: 'Test Item 1',
      description: 'A test item',
      quantity: 2,
      category: 'Test',
      minimumStockThreshold: 5
    },
  ];

  beforeEach(() => {
    getLowStockItems.mockResolvedValue(mockItems);
    updateItemQuantity.mockResolvedValue({ success: true });
  });

  it('opens edit modal and updates item quantity', async () => {
    render(<LowStockItems />);

    await waitFor(() => {
      expect(screen.getByText(/Test Item 1/i)).toBeInTheDocument();
    });

   
    fireEvent.click(screen.getByText(/Edit/i));

    // Modal should appear
    expect(screen.getByText(/Edit Quantity/i)).toBeInTheDocument();

    // Change the quantity input
    const quantityInput = screen.getByLabelText(/New Quantity/i);
    fireEvent.change(quantityInput, { target: { value: '10' } });

    // Click Update button
    fireEvent.click(screen.getByText(/Update/i));

    // Assert API was called
    await waitFor(() => {
      expect(updateItemQuantity).toHaveBeenCalledWith(1, 10);
    });

    // Modal should close after update
    await waitFor(() => {
      expect(screen.queryByText(/Edit Quantity/i)).not.toBeInTheDocument();
    });
  });
});
