import { TestBed } from "@angular/core/testing";
import {
  HttpClientTestingModule,
  HttpTestingController,
} from "@angular/common/http/testing";
import { DataService } from "./data.service";
import {
  customerMock,
  mockCustomerRead,
  mockCustomerCreate,
  mockCustomerUpdate,
  mockCustomerReadFull,
  mockAddressCreate,
  mockAddressRead,
  mockAddressReadFull,
  mockAddressUpdate,
  mockItemRead,
  mockItemCreate,
  mockItemUpdate,
  mockUomReadFull,
  mockUomCreate,
  mockUomUpdate,
  mockUomRead,
  mockItemReadFull,
} from "./mockData/mock-data";

describe("DataService", () => {
  let service: DataService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [DataService],
    });

    service = TestBed.get(DataService);
    httpMock = TestBed.get(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  // Customer
  // it("should get customers", () => {
  //   service
  //     .getCustomers()
  //     .subscribe((res) => expect(res.body).toEqual([mockCustomerRead]));
  //   const req = httpMock.expectOne(service["customerControllerLink"]);
  //   expect(req.request.method).toBe("GET");
  //   req.flush(mockCustomerRead);
  // });

  it("should get customer by ID", () => {
    service
      .getCustomer(1)
      .subscribe((res) => expect(res.body).toEqual(customerMock));
    const req = httpMock.expectOne(`${service["customerControllerLink"]}1`);
    expect(req.request.method).toBe("GET");
    req.flush(customerMock);
  });

  it("should create new customer", () => {
    service
      .newCustomer(customerMock)
      .subscribe((res) => expect(res.body).toEqual(customerMock));
    const req = httpMock.expectOne(service["customerControllerLink"]);
    expect(req.request.method).toBe("POST");
    req.flush(customerMock);
  });

  it("should update customer", () => {
    service
      .updateCustomer(1, customerMock)
      .subscribe((res) => expect(res.status).toBe(200));
    const req = httpMock.expectOne(`${service["customerControllerLink"]}1`);
    expect(req.request.method).toBe("PUT");
    req.flush({}, { status: 200, statusText: "OK" });
  });

  it("should delete customer", () => {
    service.deleteCustomer(1).subscribe((res) => expect(res.status).toBe(200));
    const req = httpMock.expectOne(`${service["customerControllerLink"]}1`);
    expect(req.request.method).toBe("DELETE");
    req.flush({}, { status: 200, statusText: "OK" });
  });

  // Address
  it("should get address by ID", () => {
    service
      .getAddress(1)
      .subscribe((res) => expect(res.body).toEqual(mockAddressReadFull));
    const req = httpMock.expectOne(`${service["addressControllerLink"]}1`);
    expect(req.request.method).toBe("GET");
    req.flush(mockAddressReadFull);
  });

  // it("should get addresses", () => {
  //   service
  //     .getAddresses()
  //     .subscribe((res) => expect(res.body).toEqual([mockAddressReadFull]));
  //   const req = httpMock.expectOne(service["addressControllerLink"]);
  //   expect(req.request.method).toBe("GET");
  //   req.flush(mockAddressReadFull);
  // });

  // it("should get addresses for customer", () => {
  //   service
  //     .getAddressesForCustomer(1)
  //     .subscribe((res) => expect(res.body).toEqual([mockAddressReadFull]));
  //   const req = httpMock.expectOne(
  //     `${service["addressControllerLink"]}forCustomer/1`
  //   );
  //   expect(req.request.method).toBe("GET");
  //   req.flush(mockAddressReadFull);
  // });

  it("should create new address", () => {
    service
      .newAddress(mockAddressCreate)
      .subscribe((res) => expect(res.body).toEqual(mockAddressReadFull));
    const req = httpMock.expectOne(service["addressControllerLink"]);
    expect(req.request.method).toBe("POST");
    req.flush(mockAddressReadFull);
  });

  it("should update address", () => {
    service
      .updateAddress(1, mockAddressUpdate)
      .subscribe((res) => expect(res.status).toBe(200));
    const req = httpMock.expectOne(`${service["addressControllerLink"]}1`);
    expect(req.request.method).toBe("PUT");
    req.flush({}, { status: 200, statusText: "OK" });
  });

  it("should delete address", () => {
    service.deleteAddress(1).subscribe((res) => expect(res.status).toBe(200));
    const req = httpMock.expectOne(`${service["addressControllerLink"]}1`);
    expect(req.request.method).toBe("DELETE");
    req.flush({}, { status: 200, statusText: "OK" });
  });

  // Items
  // it("should get items", () => {
  //   service
  //     .getItems()
  //     .subscribe((res) => expect(res.body).toEqual([mockItemReadFull]));
  //   const req = httpMock.expectOne(service["itemControllerLink"]);
  //   expect(req.request.method).toBe("GET");
  //   req.flush(mockItemReadFull);
  // });

  it("should get item by ID", () => {
    service
      .getItem(1)
      .subscribe((res) => expect(res.body).toEqual(mockItemReadFull));
    const req = httpMock.expectOne(`${service["itemControllerLink"]}1`);
    expect(req.request.method).toBe("GET");
    req.flush(mockItemReadFull);
  });

  it("should create item", () => {
    service
      .newItem(mockItemCreate)
      .subscribe((res) => expect(res.body).toEqual(mockItemReadFull));
    const req = httpMock.expectOne(service["itemControllerLink"]);
    expect(req.request.method).toBe("POST");
    req.flush(mockItemReadFull);
  });

  it("should update item", () => {
    service
      .updateItem(1, mockItemUpdate)
      .subscribe((res) => expect(res.status).toBe(200));
    const req = httpMock.expectOne(`${service["itemControllerLink"]}1`);
    expect(req.request.method).toBe("PUT");
    req.flush({}, { status: 200, statusText: "OK" });
  });

  it("should delete item", () => {
    service.deleteItem(1).subscribe((res) => expect(res.status).toBe(200));
    const req = httpMock.expectOne(`${service["itemControllerLink"]}1`);
    expect(req.request.method).toBe("DELETE");
    req.flush({}, { status: 200, statusText: "OK" });
  });

  // UOM
  // it("should get UOMs", () => {
  //   service
  //     .getUnitsOfMeasure()
  //     .subscribe((res) => expect(res.body).toEqual(mockUomRead));
  //   const req = httpMock.expectOne(service["unitOfMeasureControllerLink"]);
  //   expect(req.request.method).toBe("GET");
  //   req.flush(mockUomRead);
  // });

  //   it("should get UOM by code", () => {
  //     service
  //       .getUnitOfMeasure("KG")
  //       .subscribe((res) => expect(res.body).toEqual(uomMock));
  //     const req = httpMock.expectOne(
  //       `${service["unitOfMeasureControllerLink"]}KG`
  //     );
  //     expect(req.request.method).toBe("GET");
  //     req.flush(uomMock);
  //   });

  //   it("should create UOM", () => {
  //     service
  //       .newUnitOfMeasure(uomMock)
  //       .subscribe((res) => expect(res.body).toEqual(uomMock));
  //     const req = httpMock.expectOne(service["unitOfMeasureControllerLink"]);
  //     expect(req.request.method).toBe("POST");
  //     req.flush(uomMock);
  //   });

  //   it("should update UOM", () => {
  //     service
  //       .updateUnitOfMeasure("KG", uomMock)
  //       .subscribe((res) => expect(res.status).toBe(200));
  //     const req = httpMock.expectOne(
  //       `${service["unitOfMeasureControllerLink"]}KG`
  //     );
  //     expect(req.request.method).toBe("PUT");
  //     req.flush({}, { status: 200, statusText: "OK" });
  //   });

  //   it("should delete UOM", () => {
  //     service
  //       .deleteUnitOfMeasure("KG")
  //       .subscribe((res) => expect(res.status).toBe(200));
  //     const req = httpMock.expectOne(
  //       `${service["unitOfMeasureControllerLink"]}KG`
  //     );
  //     expect(req.request.method).toBe("DELETE");
  //     req.flush({}, { status: 200, statusText: "OK" });
  //   });

  //   // SalesOrderHeader
  //   it("should get sales orders", () => {
  //     service
  //       .getSalesOrders()
  //       .subscribe((res) => expect(res.body).toEqual(mockSalesOrderRead));
  //     const req = httpMock.expectOne(service["salesOrderHeaderControllerLink"]);
  //     expect(req.request.method).toBe("GET");
  //     req.flush(mockSalesOrderRead);
  //   });

  //   it("should get a sales order", () => {
  //     service
  //       .getSalesOrder(1)
  //       .subscribe((res) => expect(res.body).toEqual(salesOrderMock));
  //     const req = httpMock.expectOne(
  //       `${service["salesOrderHeaderControllerLink"]}1`
  //     );
  //     expect(req.request.method).toBe("GET");
  //     req.flush(salesOrderMock);
  //   });

  //   it("should create a sales order", () => {
  //     service
  //       .newSalesOrder(salesOrderMock)
  //       .subscribe((res) => expect(res.body).toEqual(salesOrderMock));
  //     const req = httpMock.expectOne(service["salesOrderHeaderControllerLink"]);
  //     expect(req.request.method).toBe("POST");
  //     req.flush(salesOrderMock);
  //   });

  //   it("should update a sales order", () => {
  //     service
  //       .updateSalesOrder(1, salesOrderMock)
  //       .subscribe((res) => expect(res.status).toBe(200));
  //     const req = httpMock.expectOne(
  //       `${service["salesOrderHeaderControllerLink"]}1`
  //     );
  //     expect(req.request.method).toBe("PUT");
  //     req.flush({}, { status: 200, statusText: "OK" });
  //   });

  //   it("should delete a sales order", () => {
  //     service
  //       .deleteSalesOrder(1)
  //       .subscribe((res) => expect(res.status).toBe(200));
  //     const req = httpMock.expectOne(
  //       `${service["salesOrderHeaderControllerLink"]}1`
  //     );
  //     expect(req.request.method).toBe("DELETE");
  //     req.flush({}, { status: 200, statusText: "OK" });
  //   });

  //   it("should update sales order profit", () => {
  //     service
  //       .updateSalesOrderProfit(1)
  //       .subscribe((res) => expect(res.body).toEqual(salesOrderMock));
  //     const req = httpMock.expectOne(
  //       `${service["salesOrderHeaderControllerLink"]}profit/1`
  //     );
  //     expect(req.request.method).toBe("GET");
  //     req.flush(salesOrderMock);
  //   });

  //   // SalesOrderLine
  //   it("should get order lines", () => {
  //     service
  //       .getOrderLines(1)
  //       .subscribe((res) => expect(res.body).toEqual(mockSalesOrderLineRead));
  //     const req = httpMock.expectOne(
  //       `${service["salesOrderLineControllerLink"]}forHeader/1`
  //     );
  //     expect(req.request.method).toBe("GET");
  //     req.flush(mockSalesOrderLineRead);
  //   });

  //   it("should get an order line", () => {
  //     service
  //       .getOrderLine(1)
  //       .subscribe((res) => expect(res.body).toEqual(salesOrderLineMock));
  //     const req = httpMock.expectOne(
  //       `${service["salesOrderLineControllerLink"]}1`
  //     );
  //     expect(req.request.method).toBe("GET");
  //     req.flush(salesOrderLineMock);
  //   });

  //   it("should create an order line", () => {
  //     service
  //       .newSalesOrderLine(salesOrderLineMock)
  //       .subscribe((res) => expect(res.body).toEqual(salesOrderLineMock));
  //     const req = httpMock.expectOne(service["salesOrderLineControllerLink"]);
  //     expect(req.request.method).toBe("POST");
  //     req.flush(salesOrderLineMock);
  //   });

  //   it("should update an order line", () => {
  //     service
  //       .updateSalesOrderLine(1, salesOrderLineMock)
  //       .subscribe((res) => expect(res.status).toBe(200));
  //     const req = httpMock.expectOne(
  //       `${service["salesOrderLineControllerLink"]}1`
  //     );
  //     expect(req.request.method).toBe("PUT");
  //     req.flush({}, { status: 200, statusText: "OK" });
  //   });

  //   it("should delete an order line", () => {
  //     service
  //       .deleteSalesOrderLine(1)
  //       .subscribe((res) => expect(res.status).toBe(200));
  //     const req = httpMock.expectOne(
  //       `${service["salesOrderLineControllerLink"]}1`
  //     );
  //     expect(req.request.method).toBe("DELETE");
  //     req.flush({}, { status: 200, statusText: "OK" });
  //   });
});