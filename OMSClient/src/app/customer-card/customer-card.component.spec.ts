import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick,
} from "@angular/core/testing";
import { CustomerCardComponent } from "./customer-card.component";
import { DataService } from "../data.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { ActivatedRoute, Router } from "@angular/router";
import { of } from "rxjs";
import { FormsModule } from "@angular/forms";
import { NO_ERRORS_SCHEMA } from "@angular/core";
import { customerMock, addressMockFull } from "../mockData/mock-data";

describe("CustomerCardComponent", () => {
  let component: CustomerCardComponent;
  let fixture: ComponentFixture<CustomerCardComponent>;
  let mockDataService: jasmine.SpyObj<DataService>;
  let mockSnackBar: jasmine.SpyObj<MatSnackBar>;
  let mockDialog: jasmine.SpyObj<MatDialog>;
  let mockRouter: jasmine.SpyObj<Router>;

  const mockRoute = {
    snapshot: {
      params: { id: 1 },
    },
  };

  beforeEach(async () => {
    mockDataService = jasmine.createSpyObj("DataService", [
      "getCustomer",
      "newCustomer",
      "updateCustomer",
      "deleteCustomer",
    ]);
    mockSnackBar = jasmine.createSpyObj("MatSnackBar", ["open"]);
    mockDialog = jasmine.createSpyObj("MatDialog", ["open"]);
    mockRouter = jasmine.createSpyObj("Router", ["navigate"]);

    await TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [CustomerCardComponent],
      providers: [
        { provide: DataService, useValue: mockDataService },
        { provide: MatSnackBar, useValue: mockSnackBar },
        { provide: MatDialog, useValue: mockDialog },
        { provide: ActivatedRoute, useValue: mockRoute },
        { provide: Router, useValue: mockRouter },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerCardComponent);
    component = fixture.componentInstance;
  });

  it("should create the component", () => {
    expect(component).toBeTruthy();
  });

  it("should fetch customer on init and set newCustomer = false", fakeAsync(() => {
    mockDataService.getCustomer.and.returnValue(
      of({ ok: true, body: customerMock })
    );

    fixture.detectChanges();
    tick();

    expect(mockDataService.getCustomer).toHaveBeenCalledWith(1);
    expect(component.customer).toEqual(customerMock);
    expect(component.newCustomer).toBeFalsy();
  }));

  it("should show snackbar if customer fetch fails", fakeAsync(() => {
    mockDataService.getCustomer.and.returnValue(of({ ok: false }));

    fixture.detectChanges();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Cannot fetch customer with id: 1",
      "OK",
      { duration: 3000 }
    );
  }));

  it("should navigate to address on goToAddress()", () => {
    const address = addressMockFull;
    component.goToAddress(address);
    expect(mockRouter.navigate).toHaveBeenCalledWith(["/Address", address.id]);
  });

  it("should create a new customer if valid and newCustomer is true", fakeAsync(() => {
    component.customer = { id: 0, name: "New Customer", addresses: [] };
    component.newCustomer = true;

    const response = {
      ok: true,
      body: { ...component.customer, id: 999 },
    };
    mockDataService.newCustomer.and.returnValue(of(response));

    component.onCustomerModified();
    tick();

    expect(mockDataService.newCustomer).toHaveBeenCalledWith(
      jasmine.objectContaining({
        id: 0,
        name: "New Customer",
        addresses: [],
      })
    );

    expect(component.customer.id).toBe(999);
    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Customer created successfully",
      "OK",
      { duration: 3000 }
    );
  }));

  it("should update customer if valid and newCustomer is false", fakeAsync(() => {
    component.customer = customerMock;
    component.newCustomer = false;

    mockDataService.updateCustomer.and.returnValue(of({ ok: true }));

    component.onCustomerModified();
    tick();

    expect(mockDataService.updateCustomer).toHaveBeenCalledWith(
      1,
      customerMock
    );
    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Customer updated successfully",
      "OK",
      { duration: 3000 }
    );
  }));

  it("should not update or create if name is empty", () => {
    component.customer = { id: 1, name: "", addresses: [] };
    component.newCustomer = false;

    component.onCustomerModified();

    expect(mockDataService.updateCustomer).not.toHaveBeenCalled();
    expect(mockDataService.newCustomer).not.toHaveBeenCalled();
  });

  it("should open dialog and delete if confirmed", fakeAsync(() => {
    component.customer = customerMock;

    const mockDialogRef = {
      afterClosed: () => of({ answer: true }),
    } as MatDialogRef<any>;

    mockDialog.open.and.returnValue(mockDialogRef);
    mockDataService.deleteCustomer.and.returnValue(of(true));

    component.delete();
    tick();

    expect(mockDataService.deleteCustomer).toHaveBeenCalledWith(1);
    expect(mockSnackBar.open).toHaveBeenCalledWith(
      `Customer "Test User" deleted successfully`,
      "OK",
      { duration: 3000 }
    );
    expect(mockRouter.navigate).toHaveBeenCalledWith(["/Customers"]);
  }));

  it("should show snackbar if deletion is cancelled", fakeAsync(() => {
    const mockDialogRef = {
      afterClosed: () => of({ answer: false }),
    } as MatDialogRef<any>;

    mockDialog.open.and.returnValue(mockDialogRef);

    component.delete();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith("Deletion aborted", "OK", {
      duration: 3000,
    });
  }));

  it("should show success snackbar on successful createCustomer", fakeAsync(() => {
    component.customer = { id: 0, name: "Test Customer", addresses: [] };
    const response = {
      ok: true,
      body: { id: 123, name: "Test Customer", addresses: [] },
    };

    mockDataService.newCustomer.and.returnValue(of(response));

    component.createCustomer();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Customer created successfully",
      "OK",
      { duration: 3000 }
    );
  }));

  it("should show failure snackbar on failed createCustomer", fakeAsync(() => {
    component.customer = { id: 0, name: "Test Customer", addresses: [] };
    const response = { ok: false };

    mockDataService.newCustomer.and.returnValue(of(response));

    component.createCustomer();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Cannot create the customer",
      "OK",
      { duration: 3000 }
    );
  }));

  it("should show success snackbar on successful updateCustomer", fakeAsync(() => {
    component.customer = { id: 1, name: "Existing Customer", addresses: [] };
    const response = { ok: true };

    mockDataService.updateCustomer.and.returnValue(of(response));

    component.updateCustomer();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Customer updated successfully",
      "OK",
      { duration: 3000 }
    );
  }));

  it("should show failure snackbar on failed updateCustomer", fakeAsync(() => {
    component.customer = { id: 1, name: "Existing Customer", addresses: [] };
    const response = { ok: false };

    mockDataService.updateCustomer.and.returnValue(of(response));

    component.updateCustomer();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Cannot update customer",
      "OK",
      { duration: 3000 }
    );
  }));
});
