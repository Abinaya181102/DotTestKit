import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick,
} from "@angular/core/testing";
import { ItemListComponent } from "./item-list.component";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Router } from "@angular/router";
import { DataService } from "../data.service";
import { of } from "rxjs";
import { MatTableModule } from "@angular/material/table";
import { MatSortModule } from "@angular/material/sort";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatRippleModule } from "@angular/material/core";
import { MatCardModule } from "@angular/material/card";
import { NoopAnimationsModule } from "@angular/platform-browser/animations";
import { MatTableDataSource } from "@angular/material/table";
import { ItemRead } from "../interfaces/item.interface";
import { mockItems } from "../mockData/mock-data";

describe("ItemListComponent", () => {
  let component: ItemListComponent;
  let fixture: ComponentFixture<ItemListComponent>;
  let mockSnackBar: jasmine.SpyObj<MatSnackBar>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockDataService: jasmine.SpyObj<DataService>;

  beforeEach(async () => {
    mockSnackBar = jasmine.createSpyObj("MatSnackBar", ["open"]);
    mockRouter = jasmine.createSpyObj("Router", ["navigate"]);
    mockDataService = jasmine.createSpyObj("DataService", ["getItems"]);

    await TestBed.configureTestingModule({
      declarations: [ItemListComponent],
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
        { provide: MatSnackBar, useValue: mockSnackBar },
        { provide: Router, useValue: mockRouter },
        { provide: DataService, useValue: mockDataService },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ItemListComponent);
    component = fixture.componentInstance;
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should fetch items on init and assign to data source", fakeAsync(() => {
    mockDataService.getItems.and.returnValue(of({ ok: true, body: mockItems }));

    fixture.detectChanges(); // triggers ngOnInit
    tick();

    expect(mockDataService.getItems).toHaveBeenCalled();
    expect(component.itemData.data).toEqual(mockItems);
  }));

  it("should show snackbar when item fetch fails", fakeAsync(() => {
    mockDataService.getItems.and.returnValue(of({ ok: false }));

    fixture.detectChanges();
    tick();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      "Cannot get items from the database",
      "OK",
      { duration: 3000 }
    );
  }));

  it("should filter itemData with input", () => {
    component.itemData = new MatTableDataSource(mockItems);
    component.applyFilter("item b");
    expect(component.itemData.filteredData.length).toBe(1);
    expect(component.itemData.filteredData[0].name).toBe("Item B");
  });

  it("should navigate to item detail on row click", () => {
    const item = mockItems[0];
    component.goToItem(item);
    expect(mockRouter.navigate).toHaveBeenCalledWith(["/Item", item.id]);
  });

  it("should show snackBar with correct message", () => {
    component.showSnackBar("Hello test");
    expect(mockSnackBar.open).toHaveBeenCalledWith("Hello test", "OK", {
      duration: 3000,
    });
  });
});
