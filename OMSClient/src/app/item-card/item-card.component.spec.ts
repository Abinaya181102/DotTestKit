import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick,
} from "@angular/core/testing";
import { ItemCardComponent } from "./item-card.component";
import { ActivatedRoute, Router } from "@angular/router";
import { DataService } from "../data.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { of } from "rxjs";
import { NO_ERRORS_SCHEMA } from "@angular/core";
import { ItemReadFull } from "../interfaces/item.interface";
import { UnitOfMeasureRead } from "../interfaces/unit-of-measure.interface";

describe("ItemCardComponent", () => {
  let component: ItemCardComponent;
  let fixture: ComponentFixture<ItemCardComponent>;
  let mockRoute: any;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockDataService: jasmine.SpyObj<DataService>;
  let mockSnackBar: jasmine.SpyObj<MatSnackBar>;

  const itemMock: ItemReadFull = {
    id: 1,
    name: "Sample Item",
    description: "Test Desc",
    unitPrice: 100,
    unitCost: 50,
    unitOfMeasureCode: "KG",
  };

  const uomsMock: UnitOfMeasureRead[] = [
    { code: "KG", name: "Kilogram" },
    { code: "L", name: "Liter" },
  ];

  beforeEach(async () => {
    mockRoute = {
      snapshot: {
        params: { id: 1 },
      },
    };

    mockRouter = jasmine.createSpyObj("Router", ["navigate"]);
    mockDataService = jasmine.createSpyObj("DataService", [
      "getItem",
      "getUnitsOfMeasure",
      "newItem",
      "updateItem",
      "deleteItem",
    ]);
    mockSnackBar = jasmine.createSpyObj("MatSnackBar", ["open"]);

    await TestBed.configureTestingModule({
      declarations: [ItemCardComponent],
      providers: [
        { provide: ActivatedRoute, useValue: mockRoute },
        { provide: Router, useValue: mockRouter },
        { provide: DataService, useValue: mockDataService },
        { provide: MatSnackBar, useValue: mockSnackBar },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();

    fixture = TestBed.createComponent(ItemCardComponent);
    component = fixture.componentInstance;
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should initialize and load item and units of measure", fakeAsync(() => {
    mockDataService.getItem.and.returnValue(of({ ok: true, body: itemMock }));
    mockDataService.getUnitsOfMeasure.and.returnValue(of({ body: uomsMock }));

    fixture.detectChanges();
    tick();

    expect(component.item).toEqual(itemMock);
    expect(component.newItem).toBeFalsy();
    expect(component.unitsOfMeasure).toEqual(uomsMock);
  }));

  it("should show snackbar if getItem fails", fakeAsync(() => {
    mockDataService.getItem.and.returnValue(of({ ok: false }));
    mockDataService.getUnitsOfMeasure.and.returnValue(of({ body: [] }));

    fixture.detectChanges();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Cannot fetch item with id: 1",
      "OK",
      { duration: 3000 }
    );
  }));

  it("should not proceed onItemModified if validation fails", () => {
    component.item = { ...itemMock, name: "", unitPrice: null, unitCost: null };
    component.newItem = true;

    const createSpy = spyOn(component, "createItem");
    const updateSpy = spyOn(component, "updateItem");

    component.onItemModified();

    expect(createSpy).not.toHaveBeenCalled();
    expect(updateSpy).not.toHaveBeenCalled();
  });

  it("should call createItem if newItem is true and valid", () => {
    component.item = itemMock;
    component.newItem = true;

    const createSpy = spyOn(component, "createItem");
    component.onItemModified();
    expect(createSpy).toHaveBeenCalled();
  });

  it("should call updateItem if newItem is false and valid", () => {
    component.item = itemMock;
    component.newItem = false;

    const updateSpy = spyOn(component, "updateItem");
    component.onItemModified();
    expect(updateSpy).toHaveBeenCalled();
  });

  it("should create item successfully", fakeAsync(() => {
    const response = { ok: true, body: itemMock };
    mockDataService.newItem.and.returnValue(of(response));
    component.item = { ...itemMock };
    component.newItem = true;

    component.createItem();
    tick();

    expect(component.item).toEqual(itemMock);
    expect(component.newItem).toBeFalsy();
    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Item created successfully",
      "OK",
      { duration: 3000 }
    );
  }));

  it("should show error if item creation fails", fakeAsync(() => {
    mockDataService.newItem.and.returnValue(of({ ok: false }));
    component.item = itemMock;
    component.newItem = true;

    component.createItem();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith("Cannot create item", "OK", {
      duration: 3000,
    });
  }));

  it("should update item successfully", fakeAsync(() => {
    mockDataService.updateItem.and.returnValue(of({ ok: true }));
    component.item = itemMock;

    component.updateItem();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Item updated successfully",
      "OK",
      { duration: 3000 }
    );
  }));

  it("should show error if item update fails", fakeAsync(() => {
    mockDataService.updateItem.and.returnValue(of({ ok: false }));
    component.item = itemMock;

    component.updateItem();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Cannot update the item",
      "OK",
      { duration: 3000 }
    );
  }));

  it("should delete item and navigate", fakeAsync(() => {
    component.item = itemMock;
    mockDataService.deleteItem.and.returnValue(of({}));

    component.deleteItem();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      `Item "Sample Item" has been successfully deleted`,
      "OK",
      { duration: 3000 }
    );
    expect(mockRouter.navigate).toHaveBeenCalledWith(["/Items"]);
  }));

  it("should show snack bar with message", () => {
    component.showSnackBar("Test Msg");
    expect(mockSnackBar.open).toHaveBeenCalledWith("Test Msg", "OK", {
      duration: 3000,
    });
  });
});
