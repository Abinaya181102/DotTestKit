import { ComponentFixture, TestBed } from "@angular/core/testing";
import { SalesOrderListComponent } from "./sales-order-list.component";
import { MatSnackBar, MatSort } from "@angular/material";
import { Router } from "@angular/router";
import { of } from "rxjs";
import { DataService } from "../data.service";
import {
  MatTableModule,
  MatFormFieldModule,
  MatInputModule,
  MatCardModule,
} from "@angular/material";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { SalesOrderHeaderRead } from "../interfaces/sales-order-header.interface";
import { CustomerRead } from "../interfaces/customer.interface";
import { AddressRead } from "../interfaces/address.interface";
import { NO_ERRORS_SCHEMA } from "@angular/core";
import { mockOrders } from "../mockData/mock-data";

describe("SalesOrderListComponent", () => {
  let component: SalesOrderListComponent;
  let fixture: ComponentFixture<SalesOrderListComponent>;
  let dataServiceSpy: jasmine.SpyObj<DataService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let snackBarSpy: jasmine.SpyObj<MatSnackBar>;

  beforeEach(async () => {
    dataServiceSpy = jasmine.createSpyObj("DataService", ["getSalesOrders"]);
    routerSpy = jasmine.createSpyObj("Router", ["navigate"]);
    snackBarSpy = jasmine.createSpyObj("MatSnackBar", ["open"]);

    await TestBed.configureTestingModule({
      declarations: [SalesOrderListComponent],
      imports: [
        MatTableModule,
        MatFormFieldModule,
        MatInputModule,
        MatCardModule,
        BrowserAnimationsModule,
      ],
      providers: [
        { provide: DataService, useValue: dataServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: MatSnackBar, useValue: snackBarSpy },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SalesOrderListComponent);
    component = fixture.componentInstance;
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should fetch sales orders and initialize table data", () => {
    dataServiceSpy.getSalesOrders.and.returnValue(
      of({ ok: true, body: mockOrders })
    );
    component.ngOnInit();
    expect(dataServiceSpy.getSalesOrders).toHaveBeenCalled();
    expect(component.orderData.data.length).toBe(1);
  });

  it("should set empty customer and address if missing", () => {
    const partialOrders: SalesOrderHeaderRead[] = [
      { id: 2, orderDate: "2025-08-02" },
    ] as any;
    dataServiceSpy.getSalesOrders.and.returnValue(
      of({ ok: true, body: partialOrders })
    );
    component.ngOnInit();
    expect(component.orderData.data[0].customer).toEqual(
      component.emptyCustomer
    );
    expect(component.orderData.data[0].address).toEqual(component.emptyAddress);
  });

  it("should show snackbar if API call fails", () => {
    dataServiceSpy.getSalesOrders.and.returnValue(of({ ok: false }));
    component.ngOnInit();
    expect(snackBarSpy.open).toHaveBeenCalledWith(
      "Cannot get orders from the database",
      "OK",
      { duration: 3000 }
    );
  });

  it("should apply filter to table data", () => {
    dataServiceSpy.getSalesOrders.and.returnValue(
      of({ ok: true, body: mockOrders })
    );
    component.ngOnInit();
    component.applyFilter("customer");
    expect(component.orderData.filter).toBe("customer");
  });

  it("should navigate to order detail page on row click", () => {
    const mockOrder = { id: 99 } as SalesOrderHeaderRead;
    component.goToOrder(mockOrder);
    expect(routerSpy.navigate).toHaveBeenCalledWith(["/SalesOrder", 99]);
  });

  it("should show snackbar with custom message", () => {
    component.showSnackBar("Test message");
    expect(snackBarSpy.open).toHaveBeenCalledWith("Test message", "OK", {
      duration: 3000,
    });
  });
});
