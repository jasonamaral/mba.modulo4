import { MatriculaCreateModel } from './../../../../models/matricula.model';
import { PagamentoCreateModel } from './../../../../models/pagamento.model';
import { Component, Inject, OnInit } from '@angular/core';
import { MatriculasService } from '../../../../services/matriculas.service';
import { CursoModel } from '../../../conteudo/models/curso.model';
import { ToastrService } from 'ngx-toastr';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from '../../../../material.module';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PagamentosService } from '../../../../services/pagamentos.service';
import { concatMap } from 'rxjs';

interface DialogData {
  curso: CursoModel;
  alunoId: string;
}

@Component({
  selector: 'app-matricula-add',
  standalone: true,
  imports: [CommonModule, MaterialModule, MatDialogModule, ReactiveFormsModule],
  templateUrl: './matricula-add.component.html',
  styleUrl: './matricula-add.component.scss'
})
export class MatriculaAddComponent implements OnInit {
  form: FormGroup;
  saving = false;
  matriculaId: string | null = null;

  constructor(
    private matriculas: MatriculasService,
    private pagamentos: PagamentosService,
    private toastr: ToastrService,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private dialogRef: MatDialogRef<MatriculaAddComponent>
  ) {
    this.form = this.fb.group({
      cursoId: [data?.curso?.id, Validators.required],
      alunoId: [data?.alunoId, Validators.required],
      numeroCartao: ['', Validators.required],
      total: [data?.curso?.valor, Validators.required],
      nomeCartao: ['', Validators.required],
      cvvCartao: ['', Validators.required],
      expiracaoCartao: ['', Validators.required]
    });
  }

  ngOnInit(): void {}

  criarMatricula() {
    this.saving = true;
    const data = this.form.value as MatriculaCreateModel;
    this.matriculas.criarMatricula(data).pipe(
      concatMap((matriculaId) => {
        this.matriculaId = matriculaId;
        this.toastr.success('Matrícula realizada com sucesso.');
        const { cursoId, ...data } = this.form.value;
        const datapagamento: PagamentoCreateModel = {
          ...data,
          matriculaId
        };
        return this.pagamentos.realizarPagamento(datapagamento);
      })
    ).subscribe({
      next: () => {
        this.toastr.success('Pagamento realizado com sucesso.');
        this.dialogRef.close({ inserted: true });
        this.saving = false;
      },
      error: (fail) => {
        const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
        if (Array.isArray(errors) && errors.length > 0) {
          this.toastr.error(errors.join('\n'));
        } else {
          this.toastr.error('Não foi possível realizar a matrícula/pagamento.');
        }
        this.saving = false;
      }
    });
  }

  save() {
    this.criarMatricula();
  }

  cancel() {
    this.form.reset();
    this.dialogRef.close({ inserted: false });
  }
}
