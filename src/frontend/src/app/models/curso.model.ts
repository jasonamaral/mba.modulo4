import { AulaModel } from './aula.model';

export interface CursoModel {
  id: string;
  nome: string;
  descricao: string;
  categoriaId: string;
  valor: number;
  createdAt: string;
  updatedAt: string;
  resumo?: string;
  nivel?: string;
  instrutor?: string;
  duracaoHoras?: number;
  imagemUrl?: string;
  nomeCategoria?: string;
  vagasMaximas?: number;
  vagasOcupadas?: number;
  vagasDisponiveis?: number;
  podeSerMatriculado?: boolean;
  validoAte?: string;
  aulas?: AulaModel[];
}


