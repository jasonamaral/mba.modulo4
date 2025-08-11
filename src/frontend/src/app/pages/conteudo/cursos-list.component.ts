import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { CursosService, CursoDto } from '../../services/cursos.service';
import { MatriculasService } from '../../services/matriculas.service';
import { MessageService } from 'src/app/services/message.service ';
import { ToastrService } from 'ngx-toastr';

@Component({
  standalone: true,
  selector: 'app-cursos-list',
  templateUrl: './cursos-list.component.html',
  imports: [CommonModule, MatCardModule, MatButtonModule, RouterModule]
})
export class CursosListComponent {
  cursos: CursoDto[] = [];
  constructor(
    private cursosService: CursosService,
    private matriculas: MatriculasService,
    private messageService: MessageService,
    private toastr: ToastrService
  ) { }
  ngOnInit() {
    this.cursosService.listar().subscribe({
      next: d => this.cursos = d,
      error: (err) => {
        const errors = (err?.error?.errors ?? err?.errors ?? []) as string[];
        if (Array.isArray(errors) && errors.length > 0) {
          errors.forEach(message => this.messageService.setMessage('Erro', message));
          this.toastr.error(errors.join('\n'));
        } else {
          this.messageService.setMessage('Erro', 'Falha ao carregar a lista de cursos. Tente novamente.');
          this.toastr.error('Falha ao carregar a lista de cursos. Tente novamente.');
        }
      }
    });
  }
  matricular(c: CursoDto) {
    this.matriculas.criarMatricula(c.id).subscribe({
      next: () => this.toastr.success('Matrícula realizada com sucesso.'),
      error: (fail) => {
        const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
        if (Array.isArray(errors) && errors.length > 0) this.toastr.error(errors.join('\n'));
        else this.toastr.error('Não foi possível realizar a matrícula.');
      }
    });
  }
}


