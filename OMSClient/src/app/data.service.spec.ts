import { TestBed } from "@angular/core/testing";
import {
  HttpClientTestingModule,
  HttpTestingController,
} from "@angular/common/http/testing";
import { DataService } from "./data.service";
import { mockCustomerRead, customerMock } from "./mockData/mock-data";
import { HttpResponse } from "@angular/common/http";

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

  //   it("should fetch customers", () => {
  //     service.getCustomers().subscribe((response) => {
  //       expect(response.body).toEqual(mockCustomerRead);
  //     });

  //     const req = httpMock.expectOne("https://localhost:5001/api/Customer/");
  //     expect(req.request.method).toBe("GET");
  //     req.flush(mockCustomerRead);
  //   });

  it("should fetch a customer by ID", () => {
    service.getCustomer(1).subscribe((response) => {
      expect(response.body).toEqual(customerMock);
    });

    const req = httpMock.expectOne("https://localhost:5001/api/Customer/1");
    expect(req.request.method).toBe("GET");
    req.flush(customerMock);
  });

  it("should delete a customer", () => {
    service.deleteCustomer(1).subscribe((response) => {
      expect(response.status).toBe(200);
    });

    const req = httpMock.expectOne("https://localhost:5001/api/Customer/1");
    expect(req.request.method).toBe("DELETE");
    req.flush({}, { status: 200, statusText: "OK" });
  });
});
