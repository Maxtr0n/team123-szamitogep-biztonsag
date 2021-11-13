import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CaffFilesComponent } from './caff-files.component';

describe('CaffFilesComponent', () => {
  let component: CaffFilesComponent;
  let fixture: ComponentFixture<CaffFilesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CaffFilesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CaffFilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
