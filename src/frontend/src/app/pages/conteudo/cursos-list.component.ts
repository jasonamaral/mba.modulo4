import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { CursosService } from '../../services/cursos.service';
import { CursoModel } from '../../models/curso.model';
import { MatriculasService } from '../../services/matriculas.service';
import { MessageService } from 'src/app/services/message.service ';
import { ToastrService } from 'ngx-toastr';
import { MatGridListModule } from '@angular/material/grid-list';
import { BreakpointObserver, LayoutModule } from '@angular/cdk/layout';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConteudoAddComponent } from './conteudo-add.component';
import { LocalStorageUtils } from 'src/app/utils/localstorage';

@Component({
  standalone: true,
  selector: 'app-cursos-list',
  templateUrl: './cursos-list.component.html',
  styleUrls: ['./cursos-list.component.scss'],
  imports: [CommonModule, MatCardModule, MatButtonModule, RouterModule, MatGridListModule, LayoutModule, MatDialogModule]
})
export class CursosListComponent {
  cursos: CursoModel[] = [];
  gridCols = 2;
  isUserAdmin = false;
  constructor(
    private cursosService: CursosService,
    private matriculas: MatriculasService,
    private messageService: MessageService,
    private toastr: ToastrService,
    private breakpointObserver: BreakpointObserver,
    private dialog: MatDialog
  ) { }
  ngOnInit() {
    // Botão de adicionar só visível para admin via template condicional
    this.isUserAdmin = new LocalStorageUtils().isUserAdmin();
    // Responsividade: 2 colunas em telas médias/maiores, 1 coluna em telas menores
    this.breakpointObserver
      .observe(['(max-width: 992px)'])
      .subscribe(state => this.gridCols = state.matches ? 1 : 2);
    this.loadCursos();
  }

  private loadCursos(): void {
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

  openAddDialog(): void {
    const ref = this.dialog.open(ConteudoAddComponent, {
      width: '1000px',
      maxWidth: '100vw',
      panelClass: ['dialog-fullwidth'],
      disableClose: true,
      autoFocus: false
    });

    ref.afterClosed().subscribe(result => {
      if (result?.inserted) {
        this.loadCursos();
      }
    });
  }
  matricular(c: CursoModel) {
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


