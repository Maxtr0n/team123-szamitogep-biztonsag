import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EditDescriptionDialogData } from 'src/app/entities/dialogData/EditDescriptionDialogData';

@Component({
  selector: 'app-edit-caff-description-dialog',
  templateUrl: './edit-caff-description-dialog.component.html',
  styleUrls: ['./edit-caff-description-dialog.component.css']
})
export class EditCaffDescriptionDialogComponent implements OnInit {

  editDescriptionForm: FormGroup; 

  constructor(public dialogRef: MatDialogRef<EditCaffDescriptionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditDescriptionDialogData,
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.editDescriptionForm = this.formBuilder.group({
      description: [null, [Validators.required]]           
    });
    this.editDescriptionForm.patchValue({
      description: this.data.description      
    });
  }

  save() {
    this.setCloseData();
    this.dialogRef.close(this.data);
  }

  setCloseData() {
    this.data.description = this.editDescriptionForm.get('description').value;    
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
