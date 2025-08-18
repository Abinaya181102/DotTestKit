import { ComponentFixture, TestBed } from "@angular/core/testing";
import { CustomerListComponent } from "./customer-list.component";
import { MatSnackBar } from "@angular/material/snack-bar";
import { DataService } from "../data.service";
import { of } from "rxjs";
import { MatTableModule } from "@angular/material/table";
import { MatSortModule } from "@angular/material/sort";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatRippleModule } from "@angular/material/core";
import { MatCardModule } from "@angular/material/card";
import { Router } from "@angular/router";
import { NoopAnimationsModule } from "@angular/platform-browser/animations";
import { CustomerRead } from "../interfaces/customer.interface";

describe("CustomerListComponent", () => {
  let component: CustomerListComponent;
  let fixture: ComponentFixture<CustomerListComponent>;
  let mockDataService: any;
  let mockSnackBar: any;
  let mockRouter: any;

  const customersMock: CustomerRead[] = [
    { id: 1, name: "Alice" },
    { id: 2, name: "Bob" },
  ];

  beforeEach(async () => {
    mockDataService = {
      getCustomers: jasmine
        .createSpy("getCustomers")
        .and.returnValue(of({ ok: true, body: customersMock })),
    };

    mockSnackBar = {
      open: jasmine.createSpy("open"),
    };

    mockRouter = {
      navigate: jasmine.createSpy("navigate"),
    };

    await TestBed.configureTestingModule({
      declarations: [CustomerListComponent],
      imports: [
        MatTableModule,
        MatSortModule,
        MatFormFieldModule,
        MatInputModule,
        MatRippleModule,
        MatCardModule,
        NoopAnimationsModule,
      ],
      providers: [
        { provide: DataService, useValue: mockDataService },
        { provide: MatSnackBar, useValue: mockSnackBar },
        { provide: Router, useValue: mockRouter },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should load customer data on init if response is ok", () => {
    expect(mockDataService.getCustomers).toHaveBeenCalled();
    expect(component.customerData.data.length).toBe(2);
  });

  it("should show snackbar if getCustomers response is not ok", () => {
    mockDataService.getCustomers.and.returnValue(of({ ok: false }));
    fixture = TestBed.createComponent(CustomerListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Cannot get customers from the database",
      "OK",
      { duration: 3000 }
    );
  });

  it("should filter customer data on applyFilter", () => {
    component.customerData.filter = "";
    component.applyFilter("alice");
    expect(component.customerData.filter).toBe("alice");
  });

  it("should navigate to customer detail page on row click", () => {
    const customer: CustomerRead = { id: 1, name: "Alice" };
    component.goToCustomer(customer);
    expect(mockRouter.navigate).toHaveBeenCalledWith(["/Customer", 1]);
  });

  it("should open snackbar with provided message", () => {
    component.showSnackBar("Test Message");
    expect(mockSnackBar.open).toHaveBeenCalledWith("Test Message", "OK", {
      duration: 3000,
    });
  });
});
