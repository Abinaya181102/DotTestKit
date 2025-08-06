import { ComponentFixture, TestBed } from "@angular/core/testing";
import { AddressListComponent } from "./address-list.component";
import { Router } from "@angular/router";
import { of } from "rxjs";
import { MatSnackBar, MatSnackBarModule } from "@angular/material/snack-bar";
import { MatSortModule } from "@angular/material/sort";
import { MatTableModule } from "@angular/material/table";
import { MatTableDataSource } from "@angular/material/table";
import { DataService } from "../data.service";
import { AddressRead } from "../interfaces/address.interface";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";

describe("AddressListComponent", () => {
  let component: AddressListComponent;
  let fixture: ComponentFixture<AddressListComponent>;
  let routerSpy = { navigate: jasmine.createSpy("navigate") };
  let mockSnackBar: any;
  let mockDataService: any;

  const dummyAddresses: AddressRead[] = [
    {
      id: 1,
      country: "India",
      postCode: "600001",
      city: "Chennai",
      street: "MG Road",
      buildingNo: "123",
    },
    {
      id: 2,
      country: "USA",
      postCode: "10001",
      city: "New York",
      street: "5th Ave",
      buildingNo: "456",
    },
  ];

  beforeEach(async () => {
    mockSnackBar = { open: jasmine.createSpy("open") };
    mockDataService = {
      getAddresses: jasmine
        .createSpy("getAddresses")
        .and.returnValue(of({ ok: true, body: dummyAddresses })),
    };

    await TestBed.configureTestingModule({
      declarations: [AddressListComponent],
      imports: [
        MatTableModule,
        MatSortModule,
        MatSnackBarModule,
        BrowserAnimationsModule,
      ],
      providers: [
        { provide: Router, useValue: routerSpy },
        { provide: DataService, useValue: mockDataService },
        { provide: MatSnackBar, useValue: mockSnackBar },
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    }).compileComponents();

    fixture = TestBed.createComponent(AddressListComponent);
    component = fixture.componentInstance;
  });

  it("should create the component", () => {
    expect(component).toBeTruthy();
  });

  it("should show snackbar if getAddresses fails", () => {
    mockDataService.getAddresses.and.returnValue(of({ ok: false }));

    fixture.detectChanges();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Cannot get addresses from database",
      "OK",
      { duration: 3000 }
    );
  });

  it("should apply filter to the datasource", () => {
    component.addressData = new MatTableDataSource(dummyAddresses);
    component.applyFilter("chennai");
    expect(component.addressData.filter).toBe("chennai");
  });

  it("should navigate to selected address", () => {
    const address = dummyAddresses[0];
    component.goToAddress(address);
    expect(routerSpy.navigate).toHaveBeenCalledWith(["/Address", address.id]);
  });

  it("should show snackbar with message", () => {
    component.showSnackBar("Test message");
    expect(mockSnackBar.open).toHaveBeenCalledWith("Test message", "OK", {
      duration: 3000,
    });
  });

  // it("should set addressData and assign sort on successful getAddresses", () => {
  //   const mockSort = {} as MatSort;
  //   component.sort = mockSort;

  //   fixture.detectChanges(); // triggers ngOnInit

  //   expect(component.addressData.data).toEqual(dummyAddresses);
  //   expect(component.addressData.sort).toBe(mockSort);
  // });
});
