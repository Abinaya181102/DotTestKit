import { ComponentFixture, TestBed } from "@angular/core/testing";
import { UnitOfMeasureListComponent } from "./unit-of-measure-list.component";
import { DataService } from "../data.service";
import {
  MatSnackBar,
  MatTableModule,
  MatFormFieldModule,
  MatInputModule,
  MatCardModule,
} from "@angular/material";
import { of } from "rxjs";
import { UnitOfMeasureRead } from "../interfaces/unit-of-measure.interface";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NO_ERRORS_SCHEMA } from "@angular/core";

describe("UnitOfMeasureListComponent", () => {
  let component: UnitOfMeasureListComponent;
  let fixture: ComponentFixture<UnitOfMeasureListComponent>;
  let dataServiceSpy: jasmine.SpyObj<DataService>;
  let snackBarSpy: jasmine.SpyObj<MatSnackBar>;

  const mockUoms: UnitOfMeasureRead[] = [
    { code: "KG", name: "Kilogram" },
    { code: "L", name: "Liter" },
  ];

  beforeEach(async () => {
    dataServiceSpy = jasmine.createSpyObj("DataService", [
      "getUnitsOfMeasure",
      "deleteUnitOfMeasure",
    ]);
    snackBarSpy = jasmine.createSpyObj("MatSnackBar", ["open"]);

    await TestBed.configureTestingModule({
      declarations: [UnitOfMeasureListComponent],
      imports: [
        MatTableModule,
        MatFormFieldModule,
        MatInputModule,
        MatCardModule,
        BrowserAnimationsModule,
      ],
      providers: [
        { provide: DataService, useValue: dataServiceSpy },
        { provide: MatSnackBar, useValue: snackBarSpy },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UnitOfMeasureListComponent);
    component = fixture.componentInstance;
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should load units of measure on init", () => {
    dataServiceSpy.getUnitsOfMeasure.and.returnValue(
      of({ ok: true, body: mockUoms })
    );
    component.ngOnInit();
    expect(dataServiceSpy.getUnitsOfMeasure).toHaveBeenCalled();
    expect(component.uomData.data.length).toBe(2);
  });

  it("should show snackbar if getUnitsOfMeasure fails", () => {
    dataServiceSpy.getUnitsOfMeasure.and.returnValue(of({ ok: false }));
    component.ngOnInit();
    expect(snackBarSpy.open).toHaveBeenCalledWith(
      "Cannot get units of measure from the database",
      "OK",
      { duration: 3000 }
    );
  });

  it("should apply filter correctly", () => {
    dataServiceSpy.getUnitsOfMeasure.and.returnValue(
      of({ ok: true, body: mockUoms })
    );
    component.ngOnInit();
    component.applyFilter("kg");
    expect(component.uomData.filter).toBe("kg");
  });

  it("should delete unit of measure and show success snackbar", () => {
    dataServiceSpy.getUnitsOfMeasure.and.returnValue(
      of({ ok: true, body: mockUoms })
    );
    dataServiceSpy.deleteUnitOfMeasure.and.returnValue(of({ ok: true }));
    component.ngOnInit();

    component.delete("KG");
    expect(dataServiceSpy.deleteUnitOfMeasure).toHaveBeenCalledWith("KG");
    expect(component.uomData.data.find((u) => u.code === "KG")).toBeUndefined();
    expect(snackBarSpy.open).toHaveBeenCalledWith(
      "Unit of Measure deleted successfully",
      "OK",
      { duration: 3000 }
    );
  });

  it("should show snackbar with custom message", () => {
    component.showSnackBar("Test message");
    expect(snackBarSpy.open).toHaveBeenCalledWith("Test message", "OK", {
      duration: 3000,
    });
  });
});
