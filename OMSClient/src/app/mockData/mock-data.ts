// import {
//   CustomerRead,
//   CustomerReadFull,
// } from "../interfaces/customer.interface";
// import { AddressRead, AddressReadFull } from "../interfaces/address.interface";
// import { ItemRead } from "../interfaces/item.interface";
// import { SalesOrderLineRead } from "../interfaces/sales-order-line.interface";
// import { SalesOrderHeaderRead } from "../interfaces/sales-order-header.interface";
// import { UnitOfMeasureRead } from "../interfaces/unit-of-measure.interface";

export const addressMockFull: AddressReadFull = {
  id: 1,
  country: "India",
  postCode: "600001",
  city: "Chennai",
  street: "Wall Street",
  buildingNo: "10",
  appartmentNo: "2A",
  customerId: 1,
};

export const customerMock: CustomerReadFull = {
  id: 1,
  name: "Test User",
  addresses: [addressMockFull],
};

export const mockItems: ItemRead[] = [
  { id: 1, name: "Item A", unitOfMeasureCode: "KG" },
  { id: 2, name: "Item B", unitOfMeasureCode: "L" },
];

import {
  CustomerCreate,
  CustomerRead,
  CustomerReadFull,
  CustomerUpdate,
} from "../interfaces/customer.interface";
import {
  AddressCreate,
  AddressRead,
  AddressReadFull,
  AddressUpdate,
} from "../interfaces/address.interface";
import {
  ItemCreate,
  ItemRead,
  ItemReadFull,
  ItemUpdate,
} from "../interfaces/item.interface";
import {
  UnitOfMeasureCreate,
  UnitOfMeasureRead,
  UnitOfMeasureReadFull,
  UnitOfMeasureUpdate,
} from "../interfaces/unit-of-measure.interface";
import {
  SalesOrderHeaderCreate,
  SalesOrderHeaderRead,
  SalesOrderHeaderReadFull,
  SalesOrderHeaderUpdate,
} from "../interfaces/sales-order-header.interface";
import {
  SalesOrderLineCreate,
  SalesOrderLineRead,
  SalesOrderLineReadFull,
  SalesOrderLineUpdate,
} from "../interfaces/sales-order-line.interface";

export const mockSalesOrderLines: SalesOrderLineRead[] = [
  {
    id: 1,
    quantity: 10,
    amount: 100,
    itemId: 1,
    salesOrderHeaderId: 101,
    item: {
      id: 1,
      name: "Item A",
      unitOfMeasureCode: "pcs",
    },
  },
];

// export const mockCustomerRead: CustomerRead[] = [
//   { id: 1, name: "Customer A" },
//   { id: 2, name: "Customer B" },
// ];

export const mockOrders: SalesOrderHeaderRead[] = [
  {
    id: 1,
    orderDate: new Date("2025-08-01"),
    customerId: 1,
    customer: {
      id: 1,
      name: "Customer 1",
    } as CustomerRead,
    addressId: 1,
    address: {
      id: 1,
      country: "India",
      city: "Chennai",
      postCode: "600001",
      street: "Mount Road",
      buildingNo: "42A",
    } as AddressRead,
  },
];

// mock-data.ts
export const mockCustomerRead: CustomerRead = {
  id: 1,
  name: "Customer A",
};

export const mockCustomerReadFull: CustomerReadFull = {
  id: 1,
  name: "Customer A",
  addresses: [],
};

export const mockCustomerCreate: CustomerCreate = {
  name: "Customer A",
};

export const mockCustomerUpdate: CustomerUpdate = {
  name: "Updated Customer A",
};

export const mockAddressRead: AddressRead = {
  id: 1,
  country: "Country A",
  postCode: "123456",
  city: "City A",
  street: "Street A",
  buildingNo: "12",
};

export const mockAddressReadFull: AddressReadFull = {
  id: 1,
  country: "Country A",
  postCode: "123456",
  city: "City A",
  street: "Street A",
  buildingNo: "12",
  appartmentNo: "101",
  customerId: 1,
};

export const mockAddressCreate: AddressCreate = {
  country: "Country A",
  postCode: "123456",
  city: "City A",
  street: "Street A",
  buildingNo: "12",
  appartmentNo: "101",
  customerId: 1,
};

export const mockAddressUpdate: AddressUpdate = {
  country: "Updated Country",
  postCode: "654321",
  city: "Updated City",
  street: "Updated Street",
  buildingNo: "34",
  appartmentNo: "202",
  customerId: 1,
};

export const mockItemRead: ItemRead = {
  id: 1,
  name: "Item A",
  unitOfMeasureCode: "PCS",
};

export const mockItemReadFull: ItemReadFull = {
  id: 1,
  name: "Item A",
  description: "Item description",
  unitOfMeasureCode: "PCS",
  unitPrice: 10,
  unitCost: 10,
};

export const mockItemCreate: ItemCreate = {
  name: "Item A",
  description: "Item description",
  unitOfMeasureCode: "PCS",
  unitPrice: 10,
  unitCost: 10,
};

export const mockItemUpdate: ItemUpdate = {
  name: "Updated Item",
  description: "Updated Description",
  unitOfMeasureCode: "PCS",
  unitPrice: 10,
  unitCost: 10,
};

export const mockUomRead: UnitOfMeasureRead = {
  code: "PCS",
  name: "Pieces",
};

export const mockUomReadFull: UnitOfMeasureReadFull = {
  code: "PCS",
  name: "Pieces",
};

export const mockUomCreate: UnitOfMeasureCreate = {
  code: "PCS",
  name: "Pieces",
};

export const mockUomUpdate: UnitOfMeasureUpdate = {
  name: "Updated Pieces",
};

// export const mockSalesOrderRead: SalesOrderHeaderRead = {
//   id: 1,
//   customerName: "Customer A",
//   orderDate: "2025-08-01",
// };

// export const mockSalesOrderReadFull: SalesOrderHeaderReadFull = {
//   id: 1,
//   customerId: 1,
//   customerName: "Customer A",
//   orderDate: "2025-08-01",
//   orderLines: [],
//   total: 200,
//   profit: 50,
// };

// export const mockSalesOrderCreate: SalesOrderHeaderCreate = {
//   customerId: 1,
//   orderDate: "2025-08-01",
// };

// export const mockSalesOrderUpdate: SalesOrderHeaderUpdate = {
//   customerId: 1,
//   orderDate: "2025-08-02",
// };

// export const mockSalesOrderLineRead: SalesOrderLineRead = {
//   id: 1,
//   : "Item A",
//   quantity: 2,
// };

// export const mockSalesOrderLineReadFull: SalesOrderLineReadFull = {
//   id: 1,
//   itemId: 1,
//   itemName: "Item A",
//   quantity: 2,
//   price: 100,
//   total: 200,
// };

// export const mockSalesOrderLineCreate: SalesOrderLineCreate = {
//   itemId: 1,
//   salesOrderHeaderId: 1,
//   quantity: 2,
// };

// export const mockSalesOrderLineUpdate: SalesOrderLineUpdate = {
//   quantity: 3,
// };
