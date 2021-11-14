import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCaffFilesComponent } from './admin-caff-files.component';

describe('AdminCaffFilesComponent', () => {
  let component: AdminCaffFilesComponent;
  let fixture: ComponentFixture<AdminCaffFilesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminCaffFilesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCaffFilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
