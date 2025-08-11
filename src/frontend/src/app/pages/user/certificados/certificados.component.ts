import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { CertificadosService, CertificadoDto } from '../../../services/certificados.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  standalone: true,
  selector: 'app-certificados',
  templateUrl: './certificados.component.html',
  imports: [CommonModule, MatTableModule, MatButtonModule]
})
export class CertificadosComponent {
  cols = ['curso','codigo','data','acoes'];
  certificados: CertificadoDto[] = [];
  constructor(private service: CertificadosService, private toastr: ToastrService) {}
  ngOnInit() {
    this.service.listar().subscribe({
      next: d => this.certificados = d,
      error: (fail) => {
        const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
        if (Array.isArray(errors) && errors.length > 0) this.toastr.error(errors.join('\n'));
        else this.toastr.error('Falha ao carregar certificados.');
      }
    });
  }
  baixar(c: CertificadoDto) { window.open(c.url, '_blank'); }
}


