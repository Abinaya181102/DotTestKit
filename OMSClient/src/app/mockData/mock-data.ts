import {
  CustomerRead,
  CustomerReadFull,
} from "../interfaces/customer.interface";
import { AddressRead, AddressReadFull } from "../interfaces/address.interface";
import { ItemRead } from "../interfaces/item.interface";
import { SalesOrderLineRead } from "../interfaces/sales-order-line.interface";
import { SalesOrderHeaderRead } from "../interfaces/sales-order-header.interface";

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

export const mockCustomerRead: CustomerRead[] = [
  { id: 1, name: "Customer A" },
  { id: 2, name: "Customer B" },
];

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
