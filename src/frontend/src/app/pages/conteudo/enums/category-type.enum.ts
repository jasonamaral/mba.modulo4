export enum CategoryTypeEnum {
    Income = 1,
    Expense = 2
}

export const CategoryTypeDescriptions: { [key in CategoryTypeEnum]: string } = {
    [CategoryTypeEnum.Income]: 'Receitas',
    [CategoryTypeEnum.Expense]: 'Despesas',
};
