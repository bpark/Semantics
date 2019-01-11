export interface DataModel {
  nodes: Node[],
  edges: Edge[]
}

// {"id":3,"label":"0.0","nodeType":"Literal"}
export interface Node {
  id: string,
  label: string,
  nodeType: string
}

// {"from":0,"to":15,"label":"tr-curr:subUnitX"}
export interface Edge {
  from: string,
  to: string,
  label: string
}

export interface VisNode extends Node {
  id: string;
  label: string;
  nodeType: string;
  color: any;
}
