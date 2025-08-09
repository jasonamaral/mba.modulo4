import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { CursosService, CursoDto } from '../../services/cursos.service';
import { MatriculasService } from '../../services/matriculas.service';

@Component({
  standalone: true,
  selector: 'app-curso-detalhe',
  templateUrl: './curso-detalhe.component.html',
  imports: [CommonModule, MatListModule, MatButtonModule]
})
export class CursoDetalheComponent {
  curso?: CursoDto;
  cursoId!: string;
  progresso = 0;
  constructor(private route: ActivatedRoute, private cursos: CursosService, private mats: MatriculasService) {}
  ngOnInit() {
    this.cursoId = this.route.snapshot.params['id'];
    this.cursos.obter(this.cursoId, true).subscribe(d => this.curso = d);
  }
  matricular() { this.mats.criarMatricula(this.cursoId).subscribe(); }
  marcarProgresso(aulaId: string) {
    this.progresso = Math.min(100, this.progresso + 10);
    this.mats.atualizarProgresso(aulaId, this.cursoId, this.progresso).subscribe();
  }
}


