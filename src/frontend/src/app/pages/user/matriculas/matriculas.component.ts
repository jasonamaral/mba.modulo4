import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatriculasService, MatriculaDto } from '../../../services/matriculas.service';

@Component({
  standalone: true,
  selector: 'app-matriculas',
  templateUrl: './matriculas.component.html',
  imports: [CommonModule, MatTableModule, MatButtonModule, MatProgressBarModule]
})
export class MatriculasComponent {
  cols = ['curso','status','progresso','acoes'];
  matriculas: MatriculaDto[] = [];

  constructor(private service: MatriculasService) {}

  ngOnInit() { this.load(); }

  load() {
    this.service.listarMatriculas().subscribe(d => this.matriculas = d);
  }

  finalizar(m: MatriculaDto) {
    this.service.finalizarCurso(m.cursoId).subscribe(() => this.load());
  }
}


