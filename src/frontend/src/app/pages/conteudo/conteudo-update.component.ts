import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule } from '@angular/common';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormControlName, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryModel } from './models/conteudo.model';
import { ConteudoService } from 'src/app/services/conteudo.service';

@Component({
  selector: 'app-conteudo-update',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MaterialModule],
  templateUrl: './conteudo-update.component.html',
})

export class ConteudoUpdateComponent extends FormBaseComponent implements OnInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  form: FormGroup = new FormGroup({});
  categoryModel!: CategoryModel;
  submitted = false;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(@Inject(MAT_DIALOG_DATA) private data: CategoryModel,
    public dialog: MatDialog,
    private categorySevice: ConteudoService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<ConteudoUpdateComponent>) {

    super();
    this.categoryModel = data;

    this.validationMessages = {
      description: {
        required: 'Informe a descrição da categoria.',
        minlength: 'A descrição precisa ter entre 4 e 100 caracteres.',
        maxlength: 'A descrição precisa ter entre 4 e 100 caracteres.',
      }
    };

    super.configureMessagesValidation(this.validationMessages);
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      description: new FormControl(this.categoryModel.description, [Validators.required, Validators.minLength(4), Validators.maxLength(100)]),
    });
  }

  ngAfterViewInit(): void {
    super.configureValidationFormBase(this.formInputElements, this.form);
  }

  submit() {
    if (!this.form.valid) return;

    this.submitted = true
    this.categoryModel = this.form.value;
    this.categoryModel.categoryId = this.data.categoryId;
    this.categoryModel.type = this.data.type;

    this.categorySevice.update(this.categoryModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {

          if (!result) {
            this.toastr.error('Erro ao salvar a categoria.');
            return;
          }

          this.toastr.success('Categoria alterada com sucesso.');
          this.dialogRef.close({ inserted: true })
        },
        error: (fail) => {
          this.submitted = false;
          const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
          this.toastr.error(Array.isArray(errors) ? errors.join('\n') : 'Erro ao salvar a categoria.');
        }
      });
  }

  cancel() {
    this.dialogRef.close({ inserted: false });
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

}
