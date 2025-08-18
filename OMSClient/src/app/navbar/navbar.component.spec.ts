import { ComponentFixture, TestBed } from "@angular/core/testing";
import { NavbarComponent } from "./navbar.component";
import { Router } from "@angular/router";
import { RouterTestingModule } from "@angular/router/testing";
import { MatMenuModule } from "@angular/material/menu";
import { MatIconModule } from "@angular/material/icon";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatButtonModule } from "@angular/material/button";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

describe("NavbarComponent", () => {
  let component: NavbarComponent;
  let fixture: ComponentFixture<NavbarComponent>;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NavbarComponent],
      imports: [
        RouterTestingModule,
        MatMenuModule,
        MatIconModule,
        BrowserAnimationsModule,
        MatToolbarModule,
        MatButtonModule,
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(NavbarComponent);
    component = fixture.componentInstance;
    router = TestBed.get(Router);
    fixture.detectChanges();
  });

  it("should create the component", () => {
    expect(component).toBeTruthy();
  });

  it("should run ngOnInit without error", () => {
    component.ngOnInit();
    expect().nothing();
  });

  it("should navigate to the provided link", () => {
    const spy = spyOn(router, "navigate");
    component.navigate("/Dashboard");
    expect(spy).toHaveBeenCalledWith(["/Dashboard"]);
  });

  it("should navigate to new SalesOrder", () => {
    const spy = spyOn(router, "navigate");
    component.newSalesOrder();
    expect(spy).toHaveBeenCalledWith(["/SalesOrder", -1]);
  });

  it("should navigate to new Customer", () => {
    const spy = spyOn(router, "navigate");
    component.newCustomer();
    expect(spy).toHaveBeenCalledWith(["/Customer", -1]);
  });

  it("should navigate to new Address", () => {
    const spy = spyOn(router, "navigate");
    component.newAddress();
    expect(spy).toHaveBeenCalledWith(["/Address", -1]);
  });

  it("should navigate to new Item", () => {
    const spy = spyOn(router, "navigate");
    component.newItem();
    expect(spy).toHaveBeenCalledWith(["/Item", -1]);
  });
});
