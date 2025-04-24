declare module 'vue3-smooth-dnd' {
    import { DefineComponent } from 'vue';

    interface DropResult {
        removedIndex: number | null;
        addedIndex: number | null;
        payload: any;
        element?: HTMLElement;
    }

    interface DropPlaceholder {
        className?: string;
        animationDuration?: number;
        showOnTop?: boolean;
    }

    interface ContainerProps {
        orientation?: 'horizontal' | 'vertical';
        behaviour?: 'move' | 'copy' | 'drop-zone' | 'contain';
        tag?: string | { value: string, props: Record<string, unknown> };
        groupName?: string;
        lockAxis?: 'x' | 'y';
        dragHandleSelector?: string;
        nonDragAreaSelector?: string;
        dragBeginDelay?: number;
        animationDuration?: number;
        autoScrollEnabled?: boolean;
        dragClass?: string;
        dropClass?: string;
        removeOnDropOut?: boolean;
        dropPlaceholder?: boolean | DropPlaceholder;
        getChildPayload?: (index: number) => any;
        shouldAnimateDrop?: (sourceContainerOptions: any, payload: any) => boolean;
        shouldAcceptDrop?: (sourceContainerOptions: any, payload: any) => boolean;
        getGhostParent?: () => HTMLElement;
        ['onDragStart']?: () => void;
        ['onDragEnd']?: () => void;
        ['onDrop']?: (dropResult: DropResult) => void;
    }

    const Container: DefineComponent<ContainerProps>;

    interface DraggableProps {
        tag?: string | { value: string, props: Record<string, unknown> };
    }

    const Draggable: DefineComponent<DraggableProps>;

    export { Container, Draggable };
}
