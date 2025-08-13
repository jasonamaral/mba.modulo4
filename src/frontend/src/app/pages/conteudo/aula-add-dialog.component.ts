import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogContent, MatDialogTitle, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from 'src/app/material.module';
import { ToastrService } from 'ngx-toastr';
import { CursosService } from 'src/app/services/cursos.service';
import { AulaCreateModel } from 'src/app/models/aula.model';

interface DialogData {
  cursoId: string;
  cursoNome?: string;
  aula?: import('src/app/models/aula.model').AulaModel;
}

@Component({
  selector: 'app-aula-add-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MaterialModule, MatDialogTitle, MatDialogContent, MatDialogActions],
  templateUrl: './aula-add-dialog.component.html',
  styleUrls: ['./aula-add-dialog.component.scss']
})
export class AulaAddDialogComponent {
  form: FormGroup = new FormGroup({
    nome: new FormControl('', [Validators.required, Validators.minLength(3)]),
    descricao: new FormControl('', [Validators.required, Validators.minLength(3)]),
    duracaoMinutos: new FormControl(0, [Validators.required, Validators.min(1)]),
    videoUrl: new FormControl('', [Validators.required])
  });

  saving = false;
  createdCount = 0;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private dialogRef: MatDialogRef<AulaAddDialogComponent>,
    private cursosService: CursosService,
    private toastr: ToastrService
  ) {
    // Pré-preenche o formulário quando "aula" for fornecida (edição)
    const aula = (this.data as any)?.aula;
    if (aula) {
      this.form.patchValue({
        nome: aula.nome ?? '',
        descricao: aula.descricao ?? '',
        duracaoMinutos: aula.duracaoMinutos ?? 0,
        videoUrl: aula.videoUrl ?? ''
      });
    }
  }

  ngOnInit(): void {
    // Se dados de aula forem fornecidos (em evolução), pré-preenche. Para agora, só curso/cursoNome
    // Mantemos a possibilidade de preencher quando editar de fato.
  }

  private buildPayload(): AulaCreateModel {
    const v = this.form.value;
    return {
      nome: v.nome,
      descricao: v.descricao,
      duracaoMinutos: Number(v.duracaoMinutos ?? 0),
      videoUrl: v.videoUrl
    } as AulaCreateModel;
  }

  save(addAnother: boolean): void {
    if (this.form.invalid || this.saving) {
      this.form.markAllAsTouched();
      return;
    }

    const payload = this.buildPayload();
    this.saving = true;
    this.cursosService.adicionarAula(this.data.cursoId, payload).subscribe({
      next: () => {
        this.toastr.success('Aula cadastrada com sucesso.');
        this.createdCount++;
        if (addAnother) {
          this.form.reset({ nome: '', descricao: '', duracaoMinutos: 0, videoUrl: '' });
        } else {
          this.dialogRef.close({ createdCount: this.createdCount });
        }
      },
      error: (e) => {
        const errors = (e?.error?.errors ?? e?.errors ?? []) as string[];
        this.toastr.error(Array.isArray(errors) ? errors.join('\n') : 'Erro ao salvar a aula.');
        this.saving = false;
      },
      complete: () => (this.saving = false)
    });
  }

  close(): void {
    this.dialogRef.close({ createdCount: this.createdCount });
  }
}


