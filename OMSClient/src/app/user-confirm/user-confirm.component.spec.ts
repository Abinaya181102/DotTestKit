import { ComponentFixture, TestBed } from "@angular/core/testing";
import { UserConfirmComponent } from "./user-confirm.component";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
import { By } from "@angular/platform-browser";
import { NO_ERRORS_SCHEMA } from "@angular/core";

describe("UserConfirmComponent", () => {
  let component: UserConfirmComponent;
  let fixture: ComponentFixture<UserConfirmComponent>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<UserConfirmComponent>>;

  beforeEach(async () => {
    dialogRefSpy = jasmine.createSpyObj("MatDialogRef", ["close"]);

    await TestBed.configureTestingModule({
      declarations: [UserConfirmComponent],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: "Are you sure?" },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserConfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create the component", () => {
    expect(component).toBeTruthy();
  });

  it("should display the question", () => {
    const content = fixture.nativeElement.querySelector("p").textContent;
    expect(content).toContain("Are you sure?");
  });

  it("should close dialog with true on closeYes()", () => {
    component.closeYes();
    expect(dialogRefSpy.close).toHaveBeenCalledWith({ answer: true });
  });

  it("should close dialog with false on closeNo()", () => {
    component.closeNo();
    expect(dialogRefSpy.close).toHaveBeenCalledWith({ answer: false });
  });

  it("should call closeYes when Yes button is clicked", () => {
    const yesBtn = fixture.debugElement.query(By.css("button:first-child"));
    yesBtn.triggerEventHandler("click", null);
    expect(dialogRefSpy.close).toHaveBeenCalledWith({ answer: true });
  });

  it("should call closeNo when No button is clicked", () => {
    const noBtn = fixture.debugElement.queryAll(By.css("button"))[1];
    noBtn.triggerEventHandler("click", null);
    expect(dialogRefSpy.close).toHaveBeenCalledWith({ answer: false });
  });
});
