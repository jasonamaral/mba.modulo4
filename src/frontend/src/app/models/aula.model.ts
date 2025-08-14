export interface AulaModel {
  id: string;
  cursoId: string;
  nome: string;
  descricao: string;
  ordem: number;
  duracaoMinutos: number;
  videoUrl: string;
  // Campos opcionais que podem vir da API e serão exibidos
  status?: string;
}

export interface AulaCreateModel {
  nome: string;
  descricao: string;
  duracaoMinutos: number;
  videoUrl: string;
}


