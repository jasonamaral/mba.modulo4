import { Component, ElementRef, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormControlName, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryModel } from 'src/app/models/conteudo.model';
import { ConteudoService } from 'src/app/services/conteudo.service';
import { CursoCreateModel } from 'src/app/models/curso.model';
import { CursosService } from 'src/app/services/cursos.service';

@Component({
  selector: 'app-conteudo-add',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MaterialModule, MatButtonModule],
  templateUrl: './conteudo-add.component.html',
})

export class ConteudoAddComponent extends FormBaseComponent implements OnInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  form: FormGroup = new FormGroup({});
  cursoModel!: CursoCreateModel;
  submitted = false;
  destroy$: Subject<boolean> = new Subject<boolean>();
  categorias: CategoryModel[] = [];

  constructor(public dialog: MatDialog,
    private categoriasService: ConteudoService,
    private cursosService: CursosService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<ConteudoAddComponent>) {

    super();

    this.validationMessages = {
      nome: {
        required: 'Informe o nome do curso.',
        minlength: 'O nome precisa ter entre 3 e 150 caracteres.',
        maxlength: 'O nome precisa ter entre 3 e 150 caracteres.',
      },
      valor: {
        required: 'Informe o valor do curso.',
        min: 'O valor deve ser maior ou igual a 0.'
      },
      duracaoHoras: {
        required: 'Informe a duração em horas.',
        min: 'A duração deve ser maior que 0.'
      },
      nivel: {
        required: 'Informe o nível do curso.'
      },
      instrutor: {
        required: 'Informe o instrutor.'
      },
      vagasMaximas: {
        required: 'Informe as vagas máximas.',
        min: 'As vagas devem ser maior que 0.'
      },
      categoriaId: {
        required: 'Selecione a categoria.'
      }
    };

    super.configureMessagesValidation(this.validationMessages);
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      nome: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]),
      valor: new FormControl(0, [Validators.required, Validators.min(0)]),
      duracaoHoras: new FormControl(0, [Validators.required, Validators.min(1)]),
      nivel: new FormControl('', [Validators.required]),
      instrutor: new FormControl('', [Validators.required]),
      vagasMaximas: new FormControl(0, [Validators.required, Validators.min(1)]),
      imagemUrl: new FormControl(''),
      validoAte: new FormControl<string | null>(null),
      categoriaId: new FormControl('', [Validators.required]),
      resumo: new FormControl(''),
      descricao: new FormControl(''),
      objetivos: new FormControl(''),
      preRequisitos: new FormControl(''),
      publicoAlvo: new FormControl(''),
      metodologia: new FormControl(''),
      recursos: new FormControl(''),
      avaliacao: new FormControl(''),
      bibliografia: new FormControl(''),
    });

    // Ajuste para novo endpoint de categorias
    this.categoriasService.getAllCategories()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (cats) => {
          const raw = (cats as any[]) ?? [];
          this.categorias = raw.map((c: any) => ({
            categoryId: c?.id ?? c?.categoryId ?? '',
            userId: '',
            description: c?.nome ?? c?.description ?? c?.descricao ?? '',
            type: 0,
          } as CategoryModel));
        },
        error: () => this.categorias = []
      });
  }

  ngAfterViewInit(): void {
    super.configureValidationFormBase(this.formInputElements, this.form);
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitted = true;
    const formValue = this.form.value;
    // Converte data para ISO, se presente
    const validoAte = formValue.validoAte ? new Date(formValue.validoAte).toISOString() : undefined;
    this.cursoModel = { ...formValue, validoAte } as CursoCreateModel;
    this.cursosService.create(this.cursoModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {

          if (!result) {
            this.toastr.error('Erro ao salvar o curso.');
            return;
          }

          this.toastr.success('Curso criado com sucesso.');
          this.dialogRef.close({ inserted: true })
        },
        error: (fail) => {
          this.submitted = false;
          const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
          this.toastr.error(Array.isArray(errors) ? errors.join('\n') : 'Erro ao salvar o curso.');
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
