import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick,
} from "@angular/core/testing";
import { SalesOrderCardComponent } from "./sales-order-card.component";
import { ActivatedRoute, Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { of } from "rxjs";
import { DataService } from "../data.service";
import { FormsModule } from "@angular/forms";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

// Angular Material Modules
import { MatTableModule } from "@angular/material/table";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from "@angular/material/button";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatNativeDateModule } from "@angular/material/core";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatTableDataSource } from "@angular/material/table";
import { MatExpansionModule } from "@angular/material/expansion";
import { mockSalesOrderLines } from "../mockData/mock-data";

describe("SalesOrderCardComponent", () => {
  let component: SalesOrderCardComponent;
  let fixture: ComponentFixture<SalesOrderCardComponent>;
  let dataServiceSpy: jasmine.SpyObj<DataService>;
  let snackBarSpy: jasmine.SpyObj<MatSnackBar>;
  let dialogSpy: jasmine.SpyObj<MatDialog>;
  let routerSpy: jasmine.SpyObj<Router>;

  const mockRoute = {
    snapshot: {
      params: {
        id: 1,
      },
    },
  };

  beforeEach(async () => {
    dataServiceSpy = jasmine.createSpyObj("DataService", [
      "getSalesOrder",
      "getItems",
      "getCustomers",
      "getAddressesForCustomer",
      "newSalesOrder",
      "updateSalesOrder",
      "deleteSalesOrder",
      "deleteSalesOrderLine",
      "newSalesOrderLine",
      "updateSalesOrderProfit",
    ]);

    snackBarSpy = jasmine.createSpyObj("MatSnackBar", ["open"]);
    dialogSpy = jasmine.createSpyObj("MatDialog", ["open"]);
    routerSpy = jasmine.createSpyObj("Router", ["navigate"]);

    await TestBed.configureTestingModule({
      declarations: [SalesOrderCardComponent],
      imports: [
        FormsModule,
        MatTableModule,
        MatInputModule,
        MatSelectModule,
        MatCardModule,
        MatIconModule,
        MatButtonModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatFormFieldModule,
        MatExpansionModule,
        BrowserAnimationsModule,
      ],
      providers: [
        { provide: ActivatedRoute, useValue: mockRoute },
        { provide: DataService, useValue: dataServiceSpy },
        { provide: MatSnackBar, useValue: snackBarSpy },
        { provide: MatDialog, useValue: dialogSpy },
        { provide: Router, useValue: routerSpy },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SalesOrderCardComponent);
    component = fixture.componentInstance;

    dataServiceSpy.getSalesOrder.and.returnValue(
      of({
        ok: true,
        body: { id: 1, customerId: 10, addressId: 100, lines: [] },
      })
    );
    dataServiceSpy.getItems.and.returnValue(of({ body: [] }));
    dataServiceSpy.getCustomers.and.returnValue(of({ body: [] }));
    dataServiceSpy.getAddressesForCustomer.and.returnValue(of({ body: [] }));

    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should fetch order, items, and customers on init", () => {
    expect(dataServiceSpy.getSalesOrder).toHaveBeenCalledWith(1);
    expect(dataServiceSpy.getItems).toHaveBeenCalled();
    expect(dataServiceSpy.getCustomers).toHaveBeenCalled();
    expect(component.newOrder).toBeFalsy();
  });

  it("should show snackbar when order fetch fails", () => {
    dataServiceSpy.getSalesOrder.and.returnValue(of({ ok: false }));
    component.setSalesOrderFromApi(2);
    expect(snackBarSpy.open).toHaveBeenCalledWith(
      "Cannot fetch order with id: 2",
      "OK",
      { duration: 3000 }
    );
  });

  it("should insert new sales order line", () => {
    component.order.id = 1;
    component.newLineItemId = 1;
    component.newLineQuantity = 5;

    const mockLine = {
      id: 2,
      itemId: 1,
      quantity: 5,
      amount: 100,
      item: { name: "Test Item" },
    };
    dataServiceSpy.newSalesOrderLine.and.returnValue(
      of({ ok: true, body: mockLine })
    );

    component.linesData = new MatTableDataSource([]);
    component.insertNewLine();

    expect(dataServiceSpy.newSalesOrderLine).toHaveBeenCalled();
    expect(component.linesData.data.length).toBe(1);
    expect(snackBarSpy.open).toHaveBeenCalledWith(
      "New line inserted successfully",
      "OK",
      { duration: 3000 }
    );
  });

  it("should delete sales order line", () => {
    component.linesData = new MatTableDataSource(mockSalesOrderLines);

    dataServiceSpy.deleteSalesOrderLine.and.returnValue(of({}));

    component.deleteLine(5);
    expect(dataServiceSpy.deleteSalesOrderLine).toHaveBeenCalledWith(5);
    expect(component.linesData.data.length).toBe(0);
  });

  it("should update order profit", () => {
    dataServiceSpy.updateSalesOrderProfit.and.returnValue(
      of({ ok: true, body: { profit: 500 } })
    );
    component.order.id = 1;
    component.updateProfit();
    expect(component.order.profit).toBe(500);
    expect(snackBarSpy.open).toHaveBeenCalledWith("Profit updated", "OK", {
      duration: 3000,
    });
  });

  it("should not update order when invalid", () => {
    component.order = {
      orderDate: undefined,
      shipmentDate: undefined,
      customerId: undefined,
      addressId: undefined,
    } as any;
    component.newOrder = false;
    component.onSalesOrderModified();
    expect(dataServiceSpy.updateSalesOrder).not.toHaveBeenCalled();
  });

  it("should create a new order", () => {
    component.newOrder = true;
    component.order = {
      orderDate: new Date(),
      shipmentDate: new Date(),
      customerId: 1,
      addressId: 1,
    } as any;
    dataServiceSpy.newSalesOrder.and.returnValue(
      of({ ok: true, body: { id: 10 } })
    );

    component.onSalesOrderModified();
    expect(dataServiceSpy.newSalesOrder).toHaveBeenCalled();
    expect(component.newOrder).toBeFalsy();
  });

  it("should update an existing order", () => {
    component.newOrder = false;
    component.order = {
      id: 5,
      orderDate: new Date(),
      shipmentDate: new Date(),
      customerId: 1,
      addressId: 1,
    } as any;
    dataServiceSpy.updateSalesOrder.and.returnValue(of({ ok: true }));

    component.onSalesOrderModified();
    expect(dataServiceSpy.updateSalesOrder).toHaveBeenCalled();
    expect(snackBarSpy.open).toHaveBeenCalledWith(
      "Order updated successfully",
      "OK",
      { duration: 3000 }
    );
  });

  it("should delete order after confirmation", fakeAsync(() => {
    const mockDialogRef = {
      afterClosed: () => of({ answer: true }),
    } as MatDialogRef<any>;

    dialogSpy.open.and.returnValue(mockDialogRef);
    dataServiceSpy.deleteSalesOrder.and.returnValue(of({ ok: true }));

    component.order.id = 1;
    component.delete();
    tick();

    expect(dataServiceSpy.deleteSalesOrder).toHaveBeenCalledWith(1);
    expect(routerSpy.navigate).toHaveBeenCalledWith(["/SalesOrders"]);
  }));

  it("should abort delete on cancel", fakeAsync(() => {
    const mockDialogRef = {
      afterClosed: () => of({ answer: false }),
    } as MatDialogRef<any>;

    dialogSpy.open.and.returnValue(mockDialogRef);

    component.delete();
    tick();

    expect(dataServiceSpy.deleteSalesOrder).not.toHaveBeenCalled();
    expect(snackBarSpy.open).toHaveBeenCalledWith("Deletion aborted", "OK", {
      duration: 3000,
    });
  }));
});
