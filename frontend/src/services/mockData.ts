import { Product, Category } from '@/types';

// Mock data for demonstration purposes
export const mockProducts: Product[] = [
  {
    id: 1,
    name: "Wireless Bluetooth Headphones",
    description: "High-quality wireless headphones with noise cancellation and 30-hour battery life.",
    price: 89.99,
    stock: 50,
    categoryId: 1
  },
  {
    id: 2,
    name: "Smartphone Case",
    description: "Durable protective case for smartphones with shock absorption technology.",
    price: 24.99,
    stock: 100,
    categoryId: 2
  },
  {
    id: 3,
    name: "Laptop Stand",
    description: "Adjustable aluminum laptop stand for better ergonomics and cooling.",
    price: 45.00,
    stock: 25,
    categoryId: 3
  },
  {
    id: 4,
    name: "Wireless Mouse",
    description: "Ergonomic wireless mouse with precision tracking and long battery life.",
    price: 35.99,
    stock: 75,
    categoryId: 1
  },
  {
    id: 5,
    name: "USB-C Cable",
    description: "High-speed USB-C cable for fast charging and data transfer.",
    price: 12.99,
    stock: 200,
    categoryId: 2
  },
  {
    id: 6,
    name: "Mechanical Keyboard",
    description: "Premium mechanical keyboard with customizable RGB lighting.",
    price: 129.99,
    stock: 30,
    categoryId: 1
  },
  {
    id: 7,
    name: "Monitor Stand",
    description: "Dual monitor stand with height adjustment and cable management.",
    price: 89.99,
    stock: 15,
    categoryId: 3
  },
  {
    id: 8,
    name: "Webcam",
    description: "4K webcam with autofocus and built-in microphone for video conferencing.",
    price: 79.99,
    stock: 40,
    categoryId: 1
  }
];

export const mockCategories: Category[] = [
  {
    id: 1,
    name: "Electronics",
    description: "Latest electronic devices and accessories",
    parentId: 0
  },
  {
    id: 2,
    name: "Accessories",
    description: "Essential accessories for your devices",
    parentId: 0
  },
  {
    id: 3,
    name: "Office Equipment",
    description: "Professional office equipment and furniture",
    parentId: 0
  },
  {
    id: 4,
    name: "Gaming",
    description: "Gaming accessories and equipment",
    parentId: 0
  },
  {
    id: 5,
    name: "Audio",
    description: "High-quality audio equipment and accessories",
    parentId: 0
  },
  {
    id: 6,
    name: "Mobile",
    description: "Mobile phone accessories and cases",
    parentId: 0
  }
];

// Mock API service methods
export const mockApiService = {
  getProducts: async () => {
    // Simulate API delay
    await new Promise(resolve => setTimeout(resolve, 500));
    return {
      succeeded: true,
      data: mockProducts,
      errors: ""
    };
  },

  getCategories: async () => {
    // Simulate API delay
    await new Promise(resolve => setTimeout(resolve, 300));
    return {
      succeeded: true,
      data: mockCategories,
      errors: ""
    };
  }
}; 