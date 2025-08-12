import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { CursosService, CursoDto } from '../../services/cursos.service';
import { MatriculasService } from '../../services/matriculas.service';

@Component({
  standalone: true,
  selector: 'app-cursos-list',
  templateUrl: './cursos-list.component.html',
  imports: [CommonModule, MatCardModule, MatButtonModule, RouterModule]
})
export class CursosListComponent {
  cursos: CursoDto[] = [];
  constructor(private cursosService: CursosService, private matriculas: MatriculasService) {}
  ngOnInit() {
    this.cursosService.listar().subscribe(d => this.cursos = d);
  }
  matricular(c: CursoDto) {
    this.matriculas.criarMatricula(c.id).subscribe();
  }
}


