import { ComponentFixture, TestBed } from "@angular/core/testing";
import { AddressCardComponent } from "./address-card.component";
import { ActivatedRoute, Router } from "@angular/router";
import { of } from "rxjs";
import { DataService } from "../data.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { NO_ERRORS_SCHEMA } from "@angular/core";

const mockRoute = {
  snapshot: {
    params: {
      id: 1,
    },
  },
};

const mockRouter = {
  navigate: jasmine.createSpy("navigate"),
};

const mockDataService = {
  getAddress: jasmine.createSpy("getAddress").and.returnValue(
    of({
      ok: true,
      body: { id: 1, country: "", postCode: "", street: "", buildingNo: "" },
    })
  ),
  getCustomers: jasmine
    .createSpy("getCustomers")
    .and.returnValue(of({ ok: true, body: [] })),
  newAddress: jasmine
    .createSpy("newAddress")
    .and.returnValue(of({ ok: true, body: { id: 1 } })),
  updateAddress: jasmine
    .createSpy("updateAddress")
    .and.returnValue(of({ ok: true })),
  deleteAddress: jasmine.createSpy("deleteAddress").and.returnValue(of(true)),
};

const mockSnackBar = {
  open: jasmine.createSpy("open"),
};

describe("AddressCardComponent", () => {
  let component: AddressCardComponent;
  let fixture: ComponentFixture<AddressCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddressCardComponent],
      providers: [
        { provide: ActivatedRoute, useValue: mockRoute },
        { provide: Router, useValue: mockRouter },
        { provide: DataService, useValue: mockDataService },
        { provide: MatSnackBar, useValue: mockSnackBar },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();

    fixture = TestBed.createComponent(AddressCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create the component", () => {
    expect(component).toBeTruthy();
  });

  it("should load address and customers on init", () => {
    expect(mockDataService.getAddress).toHaveBeenCalledWith(1);
    expect(mockDataService.getCustomers).toHaveBeenCalled();
  });

  it("should validate correctly with empty fields", () => {
    component.address = {
      country: "",
      postCode: "",
      street: "",
      buildingNo: "",
      id: 1,
    } as any;
    expect(component.validate()).toBeFalsy();
  });

  it("should create address when newAddress is true", () => {
    component.newAddress = true;
    component.address = {
      country: "IN",
      postCode: "123456",
      street: "Test St",
      buildingNo: "10",
      id: 1,
    } as any;
    spyOn(component, "validate").and.returnValue(true);
    component.onAddressModified();
    expect(mockDataService.newAddress).toHaveBeenCalled();
  });

  it("should update address when newAddress is false", () => {
    component.newAddress = false;
    component.address = {
      country: "IN",
      postCode: "123456",
      street: "Test St",
      buildingNo: "10",
      id: 1,
    } as any;
    spyOn(component, "validate").and.returnValue(true);
    component.onAddressModified();
    expect(mockDataService.updateAddress).toHaveBeenCalled();
  });

  it("should delete address and navigate", () => {
    component.address = { id: 1 } as any;
    component.deleteAddress();
    expect(mockDataService.deleteAddress).toHaveBeenCalledWith(1);
    expect(mockRouter.navigate).toHaveBeenCalledWith(["/Addresses"]);
  });
});
