import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { CursosService } from '../../services/cursos.service';
import { CursoModel } from '../../models/curso.model';
import { MatriculasService } from '../../services/matriculas.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  standalone: true,
  selector: 'app-curso-detalhe',
  templateUrl: './curso-detalhe.component.html',
  imports: [CommonModule, MatListModule, MatButtonModule]
})
export class CursoDetalheComponent {
  curso?: CursoModel;
  cursoId!: string;
  progresso = 0;
  constructor(private route: ActivatedRoute, private cursos: CursosService, private mats: MatriculasService, private toastr: ToastrService) {}
  ngOnInit() {
    this.cursoId = this.route.snapshot.params['id'];
    this.cursos.obter(this.cursoId, true).subscribe({
      next: d => this.curso = d,
      error: (fail) => {
        const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
        if (Array.isArray(errors) && errors.length > 0) this.toastr.error(errors.join('\n'));
        else this.toastr.error('Falha ao carregar detalhes do curso.');
      }
    });
  }
  matricular() {
    this.mats.criarMatricula(this.cursoId).subscribe({
      next: () => this.toastr.success('Matrícula realizada com sucesso.'),
      error: (fail) => {
        const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
        if (Array.isArray(errors) && errors.length > 0) this.toastr.error(errors.join('\n'));
        else this.toastr.error('Não foi possível realizar a matrícula.');
      }
    });
  }
  marcarProgresso(aulaId: string) {
    this.progresso = Math.min(100, this.progresso + 10);
    this.mats.atualizarProgresso(aulaId, this.cursoId, this.progresso).subscribe({
      error: (fail) => {
        const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
        if (Array.isArray(errors) && errors.length > 0) this.toastr.error(errors.join('\n'));
        else this.toastr.error('Falha ao atualizar progresso.');
      }
    });
  }
}


